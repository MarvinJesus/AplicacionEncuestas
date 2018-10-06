using CoreApi;
using CoreApi.ActionResult;
using Entities_POJO;
using Exceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Results;
using WebApi.Controllers;

namespace Survey_Integration_Test
{
    [TestClass]
    public class TopicControllerTest
    {
        private Mock<ITemaManager> _mockTopicManager;
        private TemasController _controller;

        [TestInitialize]
        public void TestInitialize()
        {
            _mockTopicManager = new Mock<ITemaManager>();

            _controller = new TemasController(_mockTopicManager.Object);
        }

        [TestMethod]
        public void GetTopic_TopicExisting_ShouldReturnOk()
        {
            _mockTopicManager.Setup(x => x.GetTopic(2))
                .Returns(new Tema { Id = 2 });

            var result = _controller.GetTopic(2, null);

            IHttpActionResult actionResult = _controller.GetTopic(2, null);
            var contentResult = actionResult as OkNegotiatedContentResult<Tema>;

            Assert.IsNotNull(contentResult);
            Assert.IsNotNull(contentResult.Content);
            Assert.AreEqual(2, contentResult.Content.Id);
        }

        [TestMethod]
        public void GetTopic_TopicNotExisting_ShoulReturnNotFound()
        {
            IHttpActionResult actionResult = _controller.GetTopic(11);

            Assert.IsInstanceOfType(actionResult, typeof(NotFoundResult));
        }

        [TestMethod]
        public void GetTopic_TopicExistingForUser_ShouldReturnOk()
        {
            _mockTopicManager.Setup(m => m.GetTopicsByUser(6))
                .Returns(new List<Tema> {
                    new Tema{ Id = 2, UsuarioId = 6},
                    new Tema{ Id = 5, UsuarioId = 6}
                });

            IHttpActionResult actionResult = _controller.GetTopic(2, 6);
            var contentResult = actionResult as OkNegotiatedContentResult<Tema>;

            Assert.IsNotNull(contentResult);
            Assert.IsNotNull(contentResult.Content);
            Assert.AreEqual(2, contentResult.Content.Id);
            Assert.AreEqual(6, contentResult.Content.UsuarioId);
        }

        [TestMethod]
        public void GetTopic_TopicNotExistingForUser_ShouldReturnNotFound()
        {
            _mockTopicManager.Setup(m => m.GetTopicsByUser(6))
                .Returns(new List<Tema> {
                    new Tema{ Id = 2, UsuarioId = 6},
                    new Tema{ Id = 5, UsuarioId = 6}
                });

            var result = _controller.GetTopic(7, 6);

            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public void GetTopicsByUser_TopicsExisting_ShouldBeReturnOk()
        {
            _mockTopicManager.Setup(t => t.GetTopicsByUser(1))
                .Returns(new List<Tema> {
                    new Tema{ Id = 1, UsuarioId = 1},
                    new Tema{Id = 2, UsuarioId = 1}
                });

            var actionResult = _controller.GetTopicsByUser(1);
            var contentResult = actionResult as OkNegotiatedContentResult<ICollection<Tema>>;

            Assert.IsNotNull(contentResult);
            Assert.IsNotNull(contentResult.Content);
            Assert.IsTrue(contentResult.Content
                .Select(t => t.UsuarioId)
                .CompareList(1));
        }

        [TestMethod]
        public void GetTopicsByUser_TopicsNotExistin_ShouldReturnNotFound()
        {
            _mockTopicManager.Setup(tc => tc.GetTopicsByUser(1))
                .Returns(new List<Tema>());

            var actionResult = _controller.GetTopicsByUser(1);
            Assert.IsInstanceOfType(actionResult, typeof(NotFoundResult));
        }

        [TestMethod]
        public void GetTopics_ThereAreTopics_ShouldReturnOk()
        {
            _mockTopicManager.Setup(tc => tc.GetTopics())
                .Returns(new List<Tema> { new Tema() });

            var actionResult = _controller.GetTopics();
            var content = actionResult as OkNegotiatedContentResult<ICollection<Tema>>;

            Assert.IsNotNull(content);
            Assert.IsNotNull(content.Content);
        }

        [TestMethod]
        public void GetTopics_ThereAreNotTopics_ShouldReturnOk()
        {
            var actionResult = _controller.GetTopics();
            var content = actionResult as OkNegotiatedContentResult<ICollection<Tema>>;

            Assert.IsNotNull(content);
            Assert.IsNull(content.Content);
        }

        [TestMethod]
        public void PostTopic_RegisterTopicSuccessfully_ShoulReturnCreated()
        {
            var topic = new Tema
            {
                Id = 1,
                Descripcion = "topic description",
                ImagePath = "Defualt.png",
                Titulo = "topic title",
                UsuarioId = 1
            };

            _mockTopicManager.Setup(tc => tc.RegistrarTema(topic))
                .Returns(new ManagerActionResult<Tema>(topic, ManagerActionStatus.Created));

            _controller.Request = new System.Net.Http.HttpRequestMessage(
                new System.Net.Http.HttpMethod("Post"),
                "http://localhost:57696/api/temas");

            var actionResult = _controller.PostTopic(topic);
            var content = actionResult as CreatedNegotiatedContentResult<Tema>;

            Assert.IsNotNull(content);
            Assert.IsNotNull(content.Content);
        }

        [TestMethod]
        public void PostTopic_RegisterTopicFailedDueToNullTopic_ShouldReturnBadRequest()
        {
            var result = _controller.PostTopic(null);

            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
        }

        [TestMethod]
        public void PostTopic_RegisterTopicFailedDueToSystemError_ShoulReturnInternalServerError()
        {
            var topic = new Tema();

            _mockTopicManager.Setup(tc => tc.RegistrarTema(topic))
                .Returns(new ManagerActionResult<Tema>(null, ManagerActionStatus.Error, new BussinessException(1)));

            var result = _controller.PostTopic(topic);

            Assert.IsInstanceOfType(result, typeof(InternalServerErrorResult));
        }

        [TestMethod]
        public void PostTopic_RegisterTopicFailedDueToIncompleteFields_ShouldReturnBadRequest()
        {
            var topic = new Tema();

            _mockTopicManager.Setup(tc => tc.RegistrarTema(topic))
                .Returns(new ManagerActionResult<Tema>(topic, ManagerActionStatus.Error,
                new BussinessException
                {
                    Code = 2,
                    AppMessage = new ApplicationMessage
                    {
                        Id = 2,
                        Message = "Por favor complete los campos requeridos",
                    }
                }));

            var result = _controller.PostTopic(topic);

            Assert.IsInstanceOfType(result, typeof(BadRequestErrorMessageResult));
        }


        [TestMethod]
        public void PutTopic_TopicUpdateFailedDueToNullTopic_ShouldReturnBadRequest()
        {
            var result = _controller.PutTopic(1, null);

            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
        }

        [TestMethod]
        public void PutTopic_TopicUpdateSuccessfully_ShouldReturnOk()
        {
            var topic = new Tema();
            _mockTopicManager.Setup(tc => tc.ActualizarTema(topic))
                .Returns(new ManagerActionResult<Tema>(topic, ManagerActionStatus.Updated));

            var actionResult = _controller.PutTopic(1, topic);
            var content = actionResult as OkNegotiatedContentResult<Tema>;

            Assert.IsNotNull(content);
            Assert.IsNotNull(content.Content);
            Assert.AreEqual(content.Content, topic);
        }

        [TestMethod]
        public void PutTopic_TopicUpdateFailedDueToDoesNotExist_ShouldReturnNotFound()
        {
            var topic = new Tema();

            _mockTopicManager.Setup(tc => tc.ActualizarTema(topic))
                .Returns(new ManagerActionResult<Tema>(topic, ManagerActionStatus.NotFound));

            var result = _controller.PutTopic(1, topic);

            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public void PutTopic_TopicUpdateFailedDueToSystemError_ShouldReturnInternalServerError()
        {
            var topic = new Tema();

            _mockTopicManager.Setup(tc => tc.ActualizarTema(topic))
                .Returns(new ManagerActionResult<Tema>(topic, ManagerActionStatus.Error));

            var result = _controller.PutTopic(1, topic);

            Assert.IsInstanceOfType(result, typeof(InternalServerErrorResult));
        }

        [TestMethod]
        public void PutTopic_TopicUpdateFailedDueToNothingHappendInTheDataBase_ShoulReturnBadRequest()
        {
            var topic = new Tema();

            _mockTopicManager.Setup(tc => tc.ActualizarTema(topic))
                .Returns(new ManagerActionResult<Tema>(topic, ManagerActionStatus.NothingModified));

            var result = _controller.PutTopic(1, topic);

            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
        }

        [TestMethod]
        public void DeleteTopic_TopicDeleteSuccessfully_ShouldReturnNoContent()
        {
            _mockTopicManager.Setup(tc => tc.DeleteTopic(1, 1))
                .Returns(new ManagerActionResult<Tema>(null, ManagerActionStatus.Deleted));

            var result = _controller.DeleteTopic(1, 1);

            Assert.IsInstanceOfType(result, typeof(StatusCodeResult));
            Assert.AreEqual((result as StatusCodeResult).StatusCode, System.Net.HttpStatusCode.NoContent);
        }

        [TestMethod]
        public void DeleteTopic_TopicDeleteFailedDueToDoesNotExist_ShoulReturNotFound()
        {
            _mockTopicManager.Setup(tc => tc.DeleteTopic(1, 1))
               .Returns(new ManagerActionResult<Tema>(null, ManagerActionStatus.NotFound));

            var result = _controller.DeleteTopic(1, 1);

            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public void DeleteTopic_TopicDeleteFailedDueToUserIdDoesNotMatchWithUserIdFromTopicinDataBase_ShouldReturnBadRequest()
        {
            _mockTopicManager.Setup(tc => tc.DeleteTopic(1, 1))
               .Returns(new ManagerActionResult<Tema>(null, ManagerActionStatus.Error));

            var result = _controller.DeleteTopic(1, 1);

            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
        }

        [TestMethod]
        public void DeleteTopic_TopicDeleteFailedDueToSystemError_ShouldReturnInternalServerError()
        {
            _mockTopicManager.Setup(tc => tc.DeleteTopic(1, 1))
                .Returns(new ManagerActionResult<Tema>(null, ManagerActionStatus.Error, new BussinessException()));

            var result = _controller.DeleteTopic(1, 1);

            Assert.IsInstanceOfType(result, typeof(InternalServerErrorResult));
        }
    }


    public static class CompareIntValues
    {
        //This mesthod return true whether the list is equals value
        public static bool CompareList(this IEnumerable<int> numbers, int numbertoCompare)
        {
            var result = true;

            foreach (var number in numbers)
            {
                if (number != numbertoCompare)
                    result = false;
            }

            return result;
        }
    }
}
