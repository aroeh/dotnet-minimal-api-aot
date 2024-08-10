using Microsoft.AspNetCore.Http.HttpResults;
using MinimalApiAot.RouteExtensions;
using System.Net;

namespace MinimalApiAot.MsTest.Tests.RouteMethods
{
    [TestClass]
    public class RestuarantV2MethodsTests
    {
        private readonly Mock<IRestuarantRepo> mockRepo;

        public RestuarantV2MethodsTests()
        {
            mockRepo = new Mock<IRestuarantRepo>();
        }


        [TestMethod]
        public async Task Get_NullRestuarants_ReturnsNotFound()
        {
            // arrange
            List<Restuarant> mockResponse = null;
            mockRepo.Setup(r => r.GetAllRestuarants()).ReturnsAsync(mockResponse);

            // act
            var testResult = await RestuarantV2Methods.GetAll(mockRepo.Object);

            // assert
            Assert.IsNotNull(testResult);
            Assert.IsInstanceOfType<NotFound>(testResult);
            Assert.AreEqual((int)HttpStatusCode.NotFound, ((NotFound)testResult).StatusCode);
        }

        [TestMethod]
        public async Task Get_EmptyRestuarants_ReturnsNotFound()
        {
            // arrange
            List<Restuarant> mockResponse = [];
            mockRepo.Setup(r => r.GetAllRestuarants()).ReturnsAsync(mockResponse);

            // act
            var testResult = await RestuarantV2Methods.GetAll(mockRepo.Object);

            // assert
            Assert.IsNotNull(testResult);
            Assert.IsInstanceOfType<NotFound>(testResult);
            Assert.AreEqual((int)HttpStatusCode.NotFound, ((NotFound)testResult).StatusCode);
        }

        [TestMethod]
        public async Task Get_HasRestuarants_ReturnsOk()
        {
            // arrange
            List<Restuarant> mockResponse =
            [
                new(),
                new()
            ];
            mockRepo.Setup(r => r.GetAllRestuarants()).ReturnsAsync(mockResponse);

            // act
            var testResult = await RestuarantV2Methods.GetAll(mockRepo.Object);

            // assert
            Assert.IsNotNull(testResult);
            Assert.IsInstanceOfType<Ok<List<Restuarant>>>(testResult);
            Assert.AreEqual((int)HttpStatusCode.OK, ((Ok<List<Restuarant>>)testResult).StatusCode);
        }

        // model state validation is not triggering and is causing incorrect results
        //[TestMethod]
        //public async Task Find_NullName_ReturnsBadRequest()
        //{
        //    // arrange
        //    SearchCriteria search = new();

        //    // act
        //    var testResult = await controller.Find(search);

        //    // assert
        //    Assert.IsNotNull(testResult);
        //    Assert.IsInstanceOfType<BadRequest>(testResult);
        //    Assert.AreEqual((int)HttpStatusCode.BadRequest, ((BadRequest)testResult).StatusCode);
        //}

        [TestMethod]
        public async Task Find_NullRestuarants_ReturnsNotFound()
        {
            // arrange
            SearchCriteria search = new()
            {
                Name = "test",
                Cuisine = "test"
            };
            List<Restuarant> mockResponse = null;
            mockRepo.Setup(r => r.FindRestuarants(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(mockResponse);

            // act
            var testResult = await RestuarantV2Methods.Find(mockRepo.Object, search);

            // assert
            Assert.IsNotNull(testResult);
            Assert.IsInstanceOfType<NotFound>(testResult);
            Assert.AreEqual((int)HttpStatusCode.NotFound, ((NotFound)testResult).StatusCode);
        }

        [TestMethod]
        public async Task Find_EmptyRestuarants_ReturnsNotFound()
        {
            // arrange
            SearchCriteria search = new()
            {
                Name = "test",
                Cuisine = "test"
            };
            List<Restuarant> mockResponse = [];
            mockRepo.Setup(r => r.FindRestuarants(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(mockResponse);

            // act
            var testResult = await RestuarantV2Methods.Find(mockRepo.Object, search);

            // assert
            Assert.IsNotNull(testResult);
            Assert.IsInstanceOfType<NotFound>(testResult);
            Assert.AreEqual((int)HttpStatusCode.NotFound, ((NotFound)testResult).StatusCode);
        }

        [TestMethod]
        public async Task Find_HasRestuarants_ReturnsOk()
        {
            // arrange
            SearchCriteria search = new()
            {
                Name = "test",
                Cuisine = "test"
            };
            List<Restuarant> mockResponse =
            [
                new(),
                new()
            ];
            mockRepo.Setup(r => r.FindRestuarants(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(mockResponse);

            // act
            var testResult = await RestuarantV2Methods.Find(mockRepo.Object, search);

            // assert
            Assert.IsNotNull(testResult);
            Assert.IsInstanceOfType<Ok<List<Restuarant>>>(testResult);
            Assert.AreEqual((int)HttpStatusCode.OK, ((Ok<List<Restuarant>>)testResult).StatusCode);
        }

        [TestMethod]
        public async Task Restuarant_NullRestuarant_ReturnsNotFound()
        {
            // arrange
            string id = "123456";
            Restuarant mockResponse = null;
            mockRepo.Setup(r => r.GetRestuarant(It.IsAny<string>())).ReturnsAsync(mockResponse);

            // act
            var testResult = await RestuarantV2Methods.GetById(mockRepo.Object, id);

            // assert
            Assert.IsNotNull(testResult);
            Assert.IsInstanceOfType<NotFound>(testResult);
            Assert.AreEqual((int)HttpStatusCode.NotFound, ((NotFound)testResult).StatusCode);
        }

        [TestMethod]
        public async Task Restuarant_RestuarantMissingId_ReturnsNotFound()
        {
            // arrange
            string id = "123456";
            Restuarant mockResponse = new();
            mockRepo.Setup(r => r.GetRestuarant(It.IsAny<string>())).ReturnsAsync(mockResponse);

            // act
            var testResult = await RestuarantV2Methods.GetById(mockRepo.Object, id);

            // assert
            Assert.IsNotNull(testResult);
            Assert.IsInstanceOfType<NotFound>(testResult);
            Assert.AreEqual((int)HttpStatusCode.NotFound, ((NotFound)testResult).StatusCode);
        }

        [TestMethod]
        public async Task Restuarant_HasRestuarants_ReturnsOk()
        {
            // arrange
            string id = "123456";
            Restuarant mockResponse = new()
            {
                Id = id,
                Name = "test",
                CuisineType = "test"
            };
            mockRepo.Setup(r => r.GetRestuarant(It.IsAny<string>())).ReturnsAsync(mockResponse);

            // act
            var testResult = await RestuarantV2Methods.GetById(mockRepo.Object, id);

            // assert
            Assert.IsNotNull(testResult);
            Assert.IsInstanceOfType<Ok<Restuarant>>(testResult);
            Assert.AreEqual((int)HttpStatusCode.OK, ((Ok<Restuarant>)testResult).StatusCode);
        }

        // TODO: See if we can figure out how to trigger model validation.  Then we can add tests for those scenarios on the Post method

        [TestMethod]
        public async Task Post_AddRestuarantFail_ReturnsOk()
        {
            // arrange
            Restuarant mockRequest = new()
            {
                Name = "test",
                CuisineType = "test"
            };
            bool mockResponse = false;
            mockRepo.Setup(r => r.InsertRestuarant(It.IsAny<Restuarant>())).ReturnsAsync(mockResponse);

            // act
            var testResult = await RestuarantV2Methods.AddRestuarant(mockRepo.Object, mockRequest);

            // assert
            Assert.IsNotNull(testResult);
            Assert.IsInstanceOfType<Ok<bool>>(testResult);
            Assert.AreEqual((int)HttpStatusCode.OK, ((Ok<bool>)testResult).StatusCode);
            Assert.IsFalse(((Ok<bool>)testResult).Value);
        }

        [TestMethod]
        public async Task Post_AddRestuarantSuccess_ReturnsOk()
        {
            // arrange
            Restuarant mockRequest = new()
            {
                Name = "test",
                CuisineType = "test"
            };
            bool mockResponse = true;
            mockRepo.Setup(r => r.InsertRestuarant(It.IsAny<Restuarant>())).ReturnsAsync(mockResponse);

            // act
            var testResult = await RestuarantV2Methods.AddRestuarant(mockRepo.Object, mockRequest);

            // assert
            Assert.IsNotNull(testResult);
            Assert.IsInstanceOfType<Ok<bool>>(testResult);
            Assert.AreEqual((int)HttpStatusCode.OK, ((Ok<bool>)testResult).StatusCode);
            Assert.IsTrue(((Ok<bool>)testResult).Value);
        }

        // TODO: See if we can figure out how to trigger model validation.  Then we can add tests for those scenarios on the Put method

        [TestMethod]
        public async Task Put_AddRestuarantFail_ReturnsOk()
        {
            // arrange
            Restuarant mockRequest = new()
            {
                Id = "123456",
                Name = "test",
                CuisineType = "test"
            };
            bool mockResponse = false;
            mockRepo.Setup(r => r.UpdateRestuarant(It.IsAny<Restuarant>())).ReturnsAsync(mockResponse);

            // act
            var testResult = await RestuarantV2Methods.UpdateRestuarant(mockRepo.Object, mockRequest);

            // assert
            Assert.IsNotNull(testResult);
            Assert.IsInstanceOfType<Ok<bool>>(testResult);
            Assert.AreEqual((int)HttpStatusCode.OK, ((Ok<bool>)testResult).StatusCode);
            Assert.IsFalse(((Ok<bool>)testResult).Value);
        }

        [TestMethod]
        public async Task Put_AddRestuarantSuccess_ReturnsOk()
        {
            // arrange
            Restuarant mockRequest = new()
            {
                Id = "123456",
                Name = "test",
                CuisineType = "test"
            };
            bool mockResponse = true;
            mockRepo.Setup(r => r.UpdateRestuarant(It.IsAny<Restuarant>())).ReturnsAsync(mockResponse);

            // act
            var testResult = await RestuarantV2Methods.UpdateRestuarant(mockRepo.Object, mockRequest);

            // assert
            Assert.IsNotNull(testResult);
            Assert.IsInstanceOfType<Ok<bool>>(testResult);
            Assert.AreEqual((int)HttpStatusCode.OK, ((Ok<bool>)testResult).StatusCode);
            Assert.IsTrue(((Ok<bool>)testResult).Value);
        }
    }
}
