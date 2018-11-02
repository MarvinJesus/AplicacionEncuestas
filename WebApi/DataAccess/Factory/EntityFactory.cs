using DataAccess.Helper;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;

namespace DataAccess.Factory
{
    public abstract class EntityFactory
    {
        protected object CreateDataShapeObject<T>(T entity, List<string> listOfFields, List<string> partialObjectsName = null)
        {
            var listOfFieldsToWorkWith = new List<string>(CleanProperties(listOfFields));
            List<string> objectsName = null;
            List<string> listOfObjectsFields = null;
            bool returnPartialObjects = false;

            if (listOfFields == null || entity == null) return entity;

            if (!listOfFieldsToWorkWith.Any()) return entity;

            if (partialObjectsName != null)
            {
                objectsName = CleanProperties(partialObjectsName);

                listOfObjectsFields = listOfFieldsToWorkWith.Where(f => f.Contains(objectsName)).ToList();

                returnPartialObjects = listOfObjectsFields.Any() && !listOfObjectsFields.Contains(objectsName);

                if (!returnPartialObjects)
                {
                    listOfObjectsFields.RemoveRange(objectsName);
                    listOfFieldsToWorkWith.RemoveRange(listOfObjectsFields);
                }
            }

            var objectToReturn = BuildObject(entity, listOfFieldsToWorkWith);


            if (returnPartialObjects)
            {
                var ObjectsFields = GetPartialObjectsFields(listOfObjectsFields, objectsName);
                var entityProperties = GetProperties(entity);

                foreach (var partialObjects in ObjectsFields)
                {
                    var result = BuildPartialObjects(entityProperties, partialObjects, entity);

                    if (result != null)
                    {
                        ((IDictionary<string, object>)objectToReturn).Add(partialObjects.Key, result);
                    }
                }
            }

            return objectToReturn;
        }

        private Dictionary<string, List<string>> GetPartialObjectsFields(List<string> listOfFields, List<string> internalObjectsName)
        {
            var listExternalObjects = new Dictionary<string, List<string>>();

            foreach (var item in listOfFields)
            {
                if (item.Contains(".") && item.Contains(internalObjectsName))
                {
                    var key = item.Substring(0, item.IndexOf("."));

                    var value = item.Substring(item.IndexOf(".") + 1).Split('.').ToList();

                    listExternalObjects.Add(key, value);
                }
            }

            return listExternalObjects;
        }

        private object BuildPartialObjects<T>(IEnumerable<string> entityProperties,
            KeyValuePair<string, List<string>> partialObjPropertiesName, T entity)
        {

            if (entityProperties.Contains(partialObjPropertiesName.Key))
            {
                var propertyValue = GetPropertyValue(entity, partialObjPropertiesName.Key);

                if (propertyValue.GetType().IsGenericType)
                {
                    System.Collections.IList objectsList = (System.Collections.IList)propertyValue;
                    var newPartialObjects = new List<object>();

                    foreach (var item in objectsList)
                    {
                        newPartialObjects.Add(BuildObject(item, partialObjPropertiesName.Value));
                    }

                    return newPartialObjects;
                }
                else
                {
                    return BuildObject(propertyValue, partialObjPropertiesName.Value);
                }
            }

            return null;
        }

        private object BuildObject<T>(T entity, List<string> listOfFields)
        {
            ExpandoObject objectToReturn = new ExpandoObject();

            var PropertiesInfo = GetProperties(entity);

            foreach (var field in listOfFields)
            {
                if (PropertiesInfo.Contains(field))
                {
                    ((IDictionary<string, object>)objectToReturn).Add(field, GetPropertyValue(entity, field));
                }
            }

            return objectToReturn;
        }

        private object GetPropertyValue<T>(T entity, string field)
        {
            return entity.GetType()
                    .GetProperty(field, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance)
                    .GetValue(entity);
        }

        private IEnumerable<string> GetProperties<T>(T entity)
        {
            return entity.GetType()
                .GetProperties(BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance)
                .Select(p => p.Name.ToLower());
        }

        private List<string> CleanProperties(List<string> source)
        {
            if (source == null) return source;

            return source.Select(p => ValidAgainstWhiteSpace(p.ToLower())).ToList();
        }

        private string ValidAgainstWhiteSpace(string item)
        {
            if (item.Contains(" "))
            {
                var newItem = item.Replace(" ", "");

                return newItem;
            }
            else
            {
                return item;
            }
        }
    }
}
