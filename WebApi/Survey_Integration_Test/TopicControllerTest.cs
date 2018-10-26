using CoreApi;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using WebApi.Controllers;

namespace Survey_Integration_Test
{
    [TestClass]
    public class TopicControllerTest
    {
        private Mock<ITopicManager> _mockTopicManager;
        private TopicsController _controller;

        [TestInitialize]
        public void TestInitialize()
        {
            _mockTopicManager = new Mock<ITopicManager>();

            _controller = new TopicsController(_mockTopicManager.Object);
        }

        /*[TestMethod]
        public void GetTopic_TopicExisting_ShouldReturnOk()
        {
            _mockTopicManager.Setup(x => x.GetTopic(2))
                .Returns(new Topic { Id = 2 });

            var result = _controller.GetTopic(2, null);

            IHttpActionResult actionResult = _controller.GetTopic(2, null);
            var contentResult = actionResult as OkNegotiatedContentResult<Topic>;

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
            _mockTopicManager.Setup(m => m.GetTopicsByUser(Guid.Parse("e6f6fe01-7de0-4301-8a5d-6b49e1eec7f1")))
                .Returns(new List<Topic> {
                    new Topic{ Id = 2, UserId = Guid.Parse("e6f6fe01-7de0-4301-8a5d-6b49e1eec7f1")},
                    new Topic{ Id = 5, UserId = Guid.Parse("e6f6fe02-7de0-4301-8a5d-6b49e1eec7f1")}
                });

            IHttpActionResult actionResult = _controller.GetTopic(2, Guid.Parse("e6f6fe01-7de0-4301-8a5d-6b49e1eec7f1"));
            var contentResult = actionResult as OkNegotiatedContentResult<Topic>;

            Assert.IsNotNull(contentResult);
            Assert.IsNotNull(contentResult.Content);
            Assert.AreEqual(2, contentResult.Content.Id);
            Assert.AreEqual(Guid.Parse("e6f6fe01-7de0-4301-8a5d-6b49e1eec7f1"), contentResult.Content.UserId);
        }

        [TestMethod]
        public void GetTopic_TopicNotExistingForUser_ShouldReturnNotFound()
        {
            _mockTopicManager.Setup(m => m.GetTopicsByUser(Guid.Parse("e6f6fe01-7de0-4301-8a5d-6b49e1eec7f1")))
                .Returns(new List<Topic> {
                    new Topic{ Id = 2, UserId = Guid.Parse("e6f6fe01-7de0-4301-8a5d-6b49e1eec7f1")},
                    new Topic{ Id = 5, UserId = Guid.Parse("e6f6fe03-7de0-4301-8a5d-6b49e1eec7f1")}
                });

            var result = _controller.GetTopic(7, Guid.Parse("e6f6fe01-7de0-4301-8a5d-6b49e1eec7f1"));

            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public void GetTopicsByUser_TopicsExisting_ShouldBeReturnOk()
        {
            _mockTopicManager.Setup(t => t.GetTopicsByUser(Guid.Parse("e6f6fe01-7de0-4301-8a5d-6b49e1eec7f1")))
                .Returns(new List<Topic> {
                    new Topic{ Id = 1, UserId = Guid.Parse("e6f6fe01-7de0-4301-8a5d-6b49e1eec7f1")},
                    new Topic{Id = 2, UserId = Guid.Parse("e6f6fe02-7de0-4301-8a5d-6b49e1eec7f1")}
                });

            var actionResult = _controller.GetTopicsByUser(Guid.Parse("e6f6fe01-7de0-4301-8a5d-6b49e1eec7f1"));
            var contentResult = actionResult as OkNegotiatedContentResult<ICollection<Topic>>;

            Assert.IsNotNull(contentResult);
            Assert.IsNotNull(contentResult.Content);
            Assert.IsTrue(contentResult.Content
                .Select(t => t.UserId)
                .CompareList(Guid.Parse("e6f6fe01-7de0-4301-8a5d-6b49e1eec7f1")));
        }

        [TestMethod]
        public void GetTopicsByUser_TopicsNotExistin_ShouldReturnNotFound()
        {
            _mockTopicManager.Setup(tc => tc.GetTopicsByUser(Guid.Parse("e6f6fe01-7de0-4301-8a5d-6b49e1eec7f1")))
                .Returns(new List<Topic>());

            var actionResult = _controller.GetTopicsByUser(Guid.Parse("e6f6fe01-7de0-4301-8a5d-6b49e1eec7f1"));
            Assert.IsInstanceOfType(actionResult, typeof(NotFoundResult));
        }

        [TestMethod]
        public void GetTopics_ThereAreTopics_ShouldReturnOk()
        {
            _mockTopicManager.Setup(tc => tc.GetTopics())
                .Returns(new List<Topic> { new Topic() });

            var actionResult = _controller.GetTopics();
            var content = actionResult as OkNegotiatedContentResult<ICollection<Topic>>;

            Assert.IsNotNull(content);
            Assert.IsNotNull(content.Content);
        }

        [TestMethod]
        public void GetTopics_ThereAreNotTopics_ShouldReturnOk()
        {
            var actionResult = _controller.GetTopics();
            var content = actionResult as OkNegotiatedContentResult<ICollection<Topic>>;

            Assert.IsNotNull(content);
            Assert.IsNull(content.Content);
        }

        [TestMethod]
        public void PostTopic_RegisterTopicSuccessfully_ShoulReturnCreated()
        {
            var topic = new Topic
            {
                Id = 1,
                Description = "topic description",
                ImagePath = "Defualt.png",
                Title = "topic title",
                UserId = Guid.Parse("e6f6fe01-7de0-4301-8a5d-6b49e1eec7f1")
            };

            _mockTopicManager.Setup(tc => tc.RegisterTopic(topic))
                .Returns(new ManagerActionResult<Topic>(topic, ManagerActionStatus.Created));

            _controller.Request = new System.Net.Http.HttpRequestMessage(
                new System.Net.Http.HttpMethod("Post"),
                "http://localhost:57067/api/Topics");

            var actionResult = _controller.PostTopic(topic);
            var content = actionResult as CreatedNegotiatedContentResult<Topic>;

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
            var topic = new Topic();

            _mockTopicManager.Setup(tc => tc.RegisterTopic(topic))
                .Returns(new ManagerActionResult<Topic>(null, ManagerActionStatus.Error, new BussinessException(1)));

            var result = _controller.PostTopic(topic);

            Assert.IsInstanceOfType(result, typeof(InternalServerErrorResult));
        }

        [TestMethod]
        public void PostTopic_RegisterTopicFailedDueToIncompleteFields_ShouldReturnBadRequest()
        {
            var topic = new Topic();

            _mockTopicManager.Setup(tc => tc.RegisterTopic(topic))
                .Returns(new ManagerActionResult<Topic>(topic, ManagerActionStatus.Error,
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
            var topic = new Topic();
            _mockTopicManager.Setup(tc => tc.EditTopic(topic))
                .Returns(new ManagerActionResult<Topic>(topic, ManagerActionStatus.Updated));

            var actionResult = _controller.PutTopic(1, topic);
            var content = actionResult as OkNegotiatedContentResult<Topic>;

            Assert.IsNotNull(content);
            Assert.IsNotNull(content.Content);
            Assert.AreEqual(content.Content, topic);
        }

        [TestMethod]
        public void PutTopic_TopicUpdateFailedDueToDoesNotExist_ShouldReturnNotFound()
        {
            var topic = new Topic();

            _mockTopicManager.Setup(tc => tc.EditTopic(topic))
                .Returns(new ManagerActionResult<Topic>(topic, ManagerActionStatus.NotFound));

            var result = _controller.PutTopic(1, topic);

            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public void PutTopic_TopicUpdateFailedDueToSystemError_ShouldReturnInternalServerError()
        {
            var topic = new Topic();

            _mockTopicManager.Setup(tc => tc.EditTopic(topic))
                .Returns(new ManagerActionResult<Topic>(topic, ManagerActionStatus.Error));

            var result = _controller.PutTopic(1, topic);

            Assert.IsInstanceOfType(result, typeof(InternalServerErrorResult));
        }

        [TestMethod]
        public void PutTopic_TopicUpdateFailedDueToNothingHappendInTheDataBase_ShoulReturnBadRequest()
        {
            var topic = new Topic();

            _mockTopicManager.Setup(tc => tc.EditTopic(topic))
                .Returns(new ManagerActionResult<Topic>(topic, ManagerActionStatus.NothingModified));

            var result = _controller.PutTopic(1, topic);

            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
        }

        [TestMethod]
        public void DeleteTopic_TopicDeleteSuccessfully_ShouldReturnNoContent()
        {
            _mockTopicManager.Setup(tc => tc.DeleteTopic(1, Guid.Parse("e6f6fe01-7de0-4301-8a5d-6b49e1eec7f1")))
                .Returns(new ManagerActionResult<Topic>(null, ManagerActionStatus.Deleted));

            var result = _controller.DeleteTopic(1, Guid.Parse("e6f6fe01-7de0-4301-8a5d-6b49e1eec7f1"));

            Assert.IsInstanceOfType(result, typeof(StatusCodeResult));
            Assert.AreEqual((result as StatusCodeResult).StatusCode, System.Net.HttpStatusCode.NoContent);
        }

        [TestMethod]
        public void DeleteTopic_TopicDeleteFailedDueToDoesNotExist_ShoulReturNotFound()
        {
            _mockTopicManager.Setup(tc => tc.DeleteTopic(1, Guid.Parse("e6f6fe01-7de0-4301-8a5d-6b49e1eec7f1")))
               .Returns(new ManagerActionResult<Topic>(null, ManagerActionStatus.NotFound));

            var result = _controller.DeleteTopic(1, Guid.Parse("e6f6fe01-7de0-4301-8a5d-6b49e1eec7f1"));

            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public void DeleteTopic_TopicDeleteFailedDueToUserIdDoesNotMatchWithUserIdFromTopicinDataBase_ShouldReturnBadRequest()
        {
            _mockTopicManager.Setup(tc => tc.DeleteTopic(1, Guid.Parse("e6f6fe01-7de0-4301-8a5d-6b49e1eec7f1")))
               .Returns(new ManagerActionResult<Topic>(null, ManagerActionStatus.Error));

            var result = _controller.DeleteTopic(1, Guid.Parse("e6f6fe01-7de0-4301-8a5d-6b49e1eec7f1"));

            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
        }

        [TestMethod]
        public void DeleteTopic_TopicDeleteFailedDueToSystemError_ShouldReturnInternalServerError()
        {
            _mockTopicManager.Setup(tc => tc.DeleteTopic(1, Guid.Parse("e6f6fe01-7de0-4301-8a5d-6b49e1eec7f1")))
                .Returns(new ManagerActionResult<Topic>(null, ManagerActionStatus.Error, new BussinessException()));

            var result = _controller.DeleteTopic(1, Guid.Parse("e6f6fe01-7de0-4301-8a5d-6b49e1eec7f1"));

            Assert.IsInstanceOfType(result, typeof(InternalServerErrorResult));
        }*/
    }


    public static class CompareGuidValues
    {
        //This mesthod return true whether the list is equals value
        public static bool CompareList(this IEnumerable<Guid> values, Guid valuetoCompare)
        {
            var result = false;

            foreach (var item in values)
            {
                if (item.ToString().Equals(valuetoCompare.ToString()))
                    result = true;
            }

            return result;
        }
    }
}
