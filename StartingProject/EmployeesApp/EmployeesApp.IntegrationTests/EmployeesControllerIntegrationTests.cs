namespace EmployeesApp.IntegrationTests
{
    // The IClassFixture interface is a decorator which indicates that tests in this class rely on
    // a fixture to run. We can see that the fixture is our TestingWebAppFactory class
    public class EmployeesControllerIntegrationTests : IClassFixture<TestingWebAppFactory<Program>>
    {
        private readonly HttpClient _client;
        public EmployeesControllerIntegrationTests(TestingWebAppFactory<Program> factory)
            => _client = factory.CreateClient();

        [Fact]
        //               [MethodUnderTest_StateUnderTest_____ExpectedBehavior]
        public async Task Index___________WhenCalled_________ReturnsApplicationForm()
        {
            // Arrange, Act
            HttpResponseMessage httpResponse = await _client.GetAsync("/Employees");

            // Assert
            httpResponse.EnsureSuccessStatusCode();

            var httpResponseAsString = await httpResponse.Content.ReadAsStringAsync();

            Assert.Contains("Mark", httpResponseAsString);
            Assert.Contains("Evelin", httpResponseAsString);
        }

        [Fact]
        public async Task Create_WhenCalled_ReturnsCreateForm()
        {
            // Arrange, Act
            var httpResponse = await _client.GetAsync("/Employees/Create");

            // Assert
            httpResponse.EnsureSuccessStatusCode();

            var responseAsString = await httpResponse.Content.ReadAsStringAsync();

            Assert.Contains("Please provide a new employee data", responseAsString);
        }

        [Fact]
        public async Task Create_SentWrongModel_ReturnsViewWithErrorMessages()
        {
            // Arrange
            var employeeFormModel = new Dictionary<string, string>
            {
                { "Name", "New Employee" },
                { "Age", "25" }
            };

            var postRequest = new HttpRequestMessage(HttpMethod.Post, "/Employees/Create")
            {
                // Manually serialize a C# object to a URL encoded string
                Content = new FormUrlEncodedContent(employeeFormModel)
            };

            // Act
            HttpResponseMessage httpResponse = await _client.SendAsync(postRequest);

            httpResponse.EnsureSuccessStatusCode();

            var responseAsString = await httpResponse.Content.ReadAsStringAsync();

            Assert.Contains("Account number is required", responseAsString);
        }

        [Fact]
        public async Task Create_PostValidEmployeeModel_ReturnsToIndexViewWithCreatedEmployee()
        {
            // rrange
            var employeeFormModel = new Dictionary<string, string>
            {
                { "Name", "New Employee" },
                { "Age", "25" },
                { "AccountNumber", "214-5874986532-21" }
            };

            var postRequest = new HttpRequestMessage(HttpMethod.Post, "/Employees/Create")
            {
                Content = new FormUrlEncodedContent(employeeFormModel)
            };

            // Act
            var httpRresponse = await _client.SendAsync(postRequest);

            // Assert
            httpRresponse.EnsureSuccessStatusCode();

            var responseAsString = await httpRresponse.Content.ReadAsStringAsync();

            Assert.Contains("New Employee", responseAsString);
            Assert.Contains("214-5874986532-21", responseAsString);
        }
    }
}