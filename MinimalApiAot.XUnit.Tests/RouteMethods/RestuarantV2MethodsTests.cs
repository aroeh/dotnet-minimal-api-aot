﻿using Microsoft.AspNetCore.Http.HttpResults;
using MinimalApiAot.RouteExtensions;
using System.Net;

namespace MinimalApiAot.XUnit.Tests.RouteMethods
{
    public class RestuarantV2MethodsTests
    {
        private readonly Mock<IRestuarantRepo> mockRepo;

        public RestuarantV2MethodsTests()
        {
            mockRepo = new Mock<IRestuarantRepo>();
        }


        [Fact]
        public async Task Get_NullRestuarants_ReturnsNotFound()
        {
            // arrange
            List<Restuarant> mockResponse = null;
            mockRepo.Setup(r => r.GetAllRestuarants()).ReturnsAsync(mockResponse);

            // act
            var testResult = await RestuarantV2Methods.GetAll(mockRepo.Object);

            // assert
            Assert.NotNull(testResult);
            Assert.IsType<NotFound>(testResult);
            Assert.Equal((int)HttpStatusCode.NotFound, ((NotFound)testResult).StatusCode);
        }

        [Fact]
        public async Task Get_EmptyRestuarants_ReturnsNotFound()
        {
            // arrange
            List<Restuarant> mockResponse = [];
            mockRepo.Setup(r => r.GetAllRestuarants()).ReturnsAsync(mockResponse);

            // act
            var testResult = await RestuarantV2Methods.GetAll(mockRepo.Object);

            // assert
            Assert.NotNull(testResult);
            Assert.IsType<NotFound>(testResult);
            Assert.Equal((int)HttpStatusCode.NotFound, ((NotFound)testResult).StatusCode);
        }

        [Fact]
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
            Assert.NotNull(testResult);
            Assert.IsType<Ok<List<Restuarant>>>(testResult);
            Assert.Equal((int)HttpStatusCode.OK, ((Ok<List<Restuarant>>)testResult).StatusCode);
        }

        // model state validation is not triggering and is causing incorrect results
        //[Fact]
        //public async Task Find_NullName_ReturnsBadRequest()
        //{
        //    // arrange
        //    SearchCriteria search = new();

        //    // act
        //    var testResult = await controller.Find(search);

        //    // assert
        //    Assert.NotNull(testResult);
        //    Assert.IsType<BadRequest>(testResult);
        //    Assert.Equal((int)HttpStatusCode.BadRequest, ((BadRequest)testResult).StatusCode);
        //}

        [Fact]
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
            Assert.NotNull(testResult);
            Assert.IsType<NotFound>(testResult);
            Assert.Equal((int)HttpStatusCode.NotFound, ((NotFound)testResult).StatusCode);
        }

        [Fact]
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
            Assert.NotNull(testResult);
            Assert.IsType<NotFound>(testResult);
            Assert.Equal((int)HttpStatusCode.NotFound, ((NotFound)testResult).StatusCode);
        }

        [Fact]
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
            Assert.NotNull(testResult);
            Assert.IsType<Ok<List<Restuarant>>>(testResult);
            Assert.Equal((int)HttpStatusCode.OK, ((Ok<List<Restuarant>>)testResult).StatusCode);
        }

        [Fact]
        public async Task Restuarant_NullRestuarant_ReturnsNotFound()
        {
            // arrange
            string id = "123456";
            Restuarant mockResponse = null;
            mockRepo.Setup(r => r.GetRestuarant(It.IsAny<string>())).ReturnsAsync(mockResponse);

            // act
            var testResult = await RestuarantV2Methods.GetById(mockRepo.Object, id);

            // assert
            Assert.NotNull(testResult);
            Assert.IsType<NotFound>(testResult);
            Assert.Equal((int)HttpStatusCode.NotFound, ((NotFound)testResult).StatusCode);
        }

        [Fact]
        public async Task Restuarant_RestuarantMissingId_ReturnsNotFound()
        {
            // arrange
            string id = "123456";
            Restuarant mockResponse = new();
            mockRepo.Setup(r => r.GetRestuarant(It.IsAny<string>())).ReturnsAsync(mockResponse);

            // act
            var testResult = await RestuarantV2Methods.GetById(mockRepo.Object, id);

            // assert
            Assert.NotNull(testResult);
            Assert.IsType<NotFound>(testResult);
            Assert.Equal((int)HttpStatusCode.NotFound, ((NotFound)testResult).StatusCode);
        }

        [Fact]
        public async Task Restuarant_MatchFound_ReturnsOk()
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
            Assert.NotNull(testResult);
            Assert.IsType<Ok<Restuarant>>(testResult);
            Assert.Equal((int)HttpStatusCode.OK, ((Ok<Restuarant>)testResult).StatusCode);
        }

        // TODO: See if we can figure out how to trigger model validation.  Then we can add tests for those scenarios on the Post method

        [Fact]
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
            Assert.NotNull(testResult);
            Assert.IsType<Ok<bool>>(testResult);
            Assert.Equal((int)HttpStatusCode.OK, ((Ok<bool>)testResult).StatusCode);
            Assert.False(((Ok<bool>)testResult).Value);
        }

        [Fact]
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
            Assert.NotNull(testResult);
            Assert.IsType<Ok<bool>>(testResult);
            Assert.Equal((int)HttpStatusCode.OK, ((Ok<bool>)testResult).StatusCode);
            Assert.True(((Ok<bool>)testResult).Value);
        }

        // TODO: See if we can figure out how to trigger model validation.  Then we can add tests for those scenarios on the Put method

        [Fact]
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
            Assert.NotNull(testResult);
            Assert.IsType<Ok<bool>>(testResult);
            Assert.Equal((int)HttpStatusCode.OK, ((Ok<bool>)testResult).StatusCode);
            Assert.False(((Ok<bool>)testResult).Value);
        }

        [Fact]
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
            Assert.NotNull(testResult);
            Assert.IsType<Ok<bool>>(testResult);
            Assert.Equal((int)HttpStatusCode.OK, ((Ok<bool>)testResult).StatusCode);
            Assert.True(((Ok<bool>)testResult).Value);
        }
    }
}
