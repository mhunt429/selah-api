﻿using Microsoft.AspNetCore.Mvc.Testing;
using Selah.WebAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using Selah.API.IntegrationTests.helpers;
using Selah.Domain.Data.Models.Authentication;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;

namespace Selah.API.IntegrationTests.ControllerTests
{
    public class AuthControllerTest
    {
        private SelahApiTestFactory _testFactory;
        public AuthControllerTest()
        {
            _testFactory = new SelahApiTestFactory();
            _testFactory.CreateClient();
        }


        [Fact]
        public async Task Should_Return_Unauthorized_On_Invalid_Login()
        {
            //Arrange
            var client = _testFactory.CreateClient();
            var login = new AuthenticationRequest { EmailOrUsername = "bad_user", Password = "BadP@ssword" };
            var requestBody = JsonConvert.SerializeObject(login, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            });
            var httpContent = new StringContent(requestBody, Encoding.UTF8, "application/json");

            //Act
            var response = await client.PostAsync("api/v1/oauth/login", httpContent);

            //Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.Unauthorized);
        }
    }
}
