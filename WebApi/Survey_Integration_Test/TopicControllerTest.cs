using CoreApi;
using Entities_POJO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Results;
using WebApi.Controllers;

namespace Survey_Integration_Test
{
    [TestClass]
    public class TopicControllerTest
    {
        private Mock<ITemaManager> _mockTopicManager;
        private Mock<IUsuarioManager> _mockUserManager;
        private TemasController _controller;

        [TestInitialize]
        public void TestInitialize()
        {
            _mockTopicManager = new Mock<ITemaManager>();
            _mockUserManager = new Mock<IUsuarioManager>();

            _controller = new TemasController(_mockTopicManager.Object, _mockUserManager.Object);
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
    }
}
