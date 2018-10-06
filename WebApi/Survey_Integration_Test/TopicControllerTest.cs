using CoreApi;
using Entities_POJO;
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
    }

    public static class CompareIntValues
    {
        //This mesthod return true whether the list is equals value
        public static bool CompareList(this IEnumerable<int> numbers, int value)
        {
            var result = true;

            foreach (var number in numbers)
            {
                if (number != value)
                    result = false;
            }

            return result;
        }
    }
}
