# Milestone V-1: Integration Testing Infrastructure

**Status:** 📋 Planned
**Priority:** MEDIUM (4/10 pain, future confidence building)
**Estimated Effort:** 1-2 weeks

---

**Objective:** Establish HTTP-layer test coverage for Azure Functions endpoints to verify end-to-end request/response behavior, middleware pipeline, and runtime interactions.

**Success Criteria:**
- ~8-10 integration tests passing in CI for GetDailyContent and ReactToContent endpoints
- Azurite service running in GitHub Actions workflow (npm-based, no Docker required)
- Test-specific fixtures isolated from production samples
- Testing documentation created in docs/testing/
- Zero-warning build maintained

---

## Feature V-1.01: Integration Test Project Setup & Azurite Service
*Establish test project infrastructure with Azure Functions in-process hosting and Azurite emulator.*

- [ ] **Story V-1.1.1:** As a developer, I want an integration test project with Azure Functions hosting so that I can test HTTP endpoints without deploying to Azure.
    - **AC 1:** Create `src/Aurora.Api.IntegrationTests/Aurora.Api.IntegrationTests.csproj` (xUnit, .NET 9)
    - **AC 2:** Add NuGet packages:
        - `Microsoft.AspNetCore.Mvc.Testing` (for WebApplicationFactory pattern)
        - `Microsoft.Azure.Functions.Worker.Extensions.Http` (for HTTP trigger support)
        - `Azure.Storage.Blobs` and `Azure.Data.Tables` (for Azurite interaction)
        - `xUnit` and `FluentAssertions` (testing frameworks)
    - **AC 3:** Create `TestWebApplicationFactory.cs` that:
        - Hosts Aurora.Api functions in-process
        - Configures Azurite connection strings (`UseDevelopmentStorage=true`)
        - Overrides configuration for test environment
    - **AC 4:** Create `IntegrationTestBase.cs` base class:
        - Manages test server lifecycle
        - Provides `HttpClient` for endpoint calls
        - Provides `BlobServiceClient` and `TableServiceClient` for test data setup
        - Implements `IAsyncLifetime` for setup/teardown
    - **AC 5:** Add to `Project-Aurora.sln` under `tests/` solution folder
    - **AC 6:** Verify empty test project builds with zero warnings
    - **Edge Cases:**
        - In-process hosting limitations: Document scenarios that require deployed testing
        - Port conflicts: Use dynamic port allocation for test server
    - **Time Estimate:** 4 hours

- [ ] **Story V-1.1.2:** As a developer, I want Azurite installed and running in my local development environment so that integration tests can access emulated Azure Storage.
    - **AC 1:** Document Azurite installation in `docs/testing/INTEGRATION_TESTING.md`:
        ```bash
        # Install Azurite via npm (global)
        npm install -g azurite

        # Start Azurite (runs until stopped)
        azurite --silent --location ./azurite-data --debug ./azurite-debug.log
        ```
    - **AC 2:** Add `azurite-data/` and `azurite-debug.log` to `.gitignore`
    - **AC 3:** Create `tools/start-azurite.ps1` helper script:
        ```powershell
        # Check if Azurite is running
        # If not, start in background
        # Output: "Azurite started" or "Azurite already running"
        ```
    - **AC 4:** Integration test setup checks Azurite availability:
        - Attempt to connect to `http://127.0.0.1:10000` (Azurite blob endpoint)
        - If unavailable, skip tests with clear message: "Azurite not running. Start with: azurite"
    - **AC 5:** Document Windows, macOS, Linux installation differences
    - **Edge Cases:**
        - Azurite port conflicts (10000-10002): Document port configuration options
        - npm not installed: Provide download link for Node.js in error message
        - Azurite version compatibility: Specify minimum version (e.g., 3.30.0+)
    - **Time Estimate:** 2 hours

- [ ] **Story V-1.1.3:** As a developer, I want Azurite running in GitHub Actions CI so that integration tests execute automatically on every push.
    - **AC 1:** Update `.github/workflows/dotnet.yml` to install and start Azurite:
        ```yaml
        - name: Install Azurite
          run: npm install -g azurite

        - name: Start Azurite
          run: azurite --silent --location ./azurite-data &

        - name: Wait for Azurite
          run: npx wait-on http://127.0.0.1:10000
        ```
    - **AC 2:** Add integration test execution step:
        ```yaml
        - name: Run Integration Tests
          run: dotnet test src/Aurora.Api.IntegrationTests --no-build --verbosity normal
        ```
    - **AC 3:** Ensure Azurite starts before integration tests run (use `wait-on` npm package)
    - **AC 4:** Verify workflow succeeds with integration tests passing
    - **Edge Cases:**
        - Azurite startup timeout: Allow 10 seconds for startup, fail with timeout error if unavailable
        - Port already in use (CI runner collision): Document retry mechanism or dynamic port allocation
        - Workflow fails on Windows/Linux/macOS matrix: Test across all platforms if multi-platform CI configured
    - **Time Estimate:** 2 hours

- [ ] **Story V-1.1.4:** As a developer, I want test-specific content fixtures so that integration tests are isolated from production samples.
    - **AC 1:** Create `src/Aurora.Api.IntegrationTests/Fixtures/` directory
    - **AC 2:** Create test fixtures:
        - `test-content.json`: Minimal valid content (1 Vibe + 3 Picks) for success scenarios
        - `invalid-content.json`: Malformed JSON for error scenario testing
        - `empty-content.json`: Empty object for edge case testing
    - **AC 3:** Create `TestDataSeeder.cs` helper class:
        - Method `SeedBlobStorageAsync()`: Uploads test-content.json to Azurite
        - Method `SeedTableStorageAsync()`: Creates test reaction entities
        - Method `CleanupAsync()`: Deletes all test data after test run
    - **AC 4:** `IntegrationTestBase` calls seeder in setup/teardown:
        ```csharp
        public async Task InitializeAsync()
        {
            await TestDataSeeder.SeedBlobStorageAsync(BlobServiceClient);
            await TestDataSeeder.SeedTableStorageAsync(TableServiceClient);
        }

        public async Task DisposeAsync()
        {
            await TestDataSeeder.CleanupAsync(BlobServiceClient, TableServiceClient);
        }
        ```
    - **AC 5:** All fixtures marked as "Copy to Output Directory" in .csproj
    - **Edge Cases:**
        - Seeding fails mid-test: Cleanup runs even if test throws exception
        - Multiple tests run in parallel: Each test uses unique container/table names (append random GUID)
    - **Time Estimate:** 3 hours

---

## Feature V-1.02: GetDailyContent Integration Tests
*Comprehensive HTTP-layer tests for content delivery endpoint covering success, error, and retry scenarios.*

- [ ] **Story V-1.2.1:** As a developer, I want integration tests for GetDailyContent success scenarios so that I can verify correct content delivery.
    - **AC 1:** Create `src/Aurora.Api.IntegrationTests/GetDailyContentTests.cs`
    - **AC 2:** Test: `GetDailyContent_ValidBlob_Returns200WithContent`
        - Seed test-content.json to Azurite
        - Call `GET /api/GetDailyContent`
        - Assert HTTP 200 status
        - Assert Content-Type: application/json
        - Assert response body deserializes to ContentFeed
        - Assert Vibe and Picks counts match seeded data
    - **AC 3:** Test: `GetDailyContent_ColdStart_ReturnsWithinTimeout`
        - First call to endpoint (cold start scenario)
        - Assert response time <5 seconds (generous timeout for Functions cold start)
        - Note: This is functional, not performance benchmark (defer strict perf to separate feature)
    - **AC 4:** Use FluentAssertions for readable assertions:
        ```csharp
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        content.VibeOfTheDay.Should().NotBeNull();
        content.DailyPicks.Should().HaveCount(3);
        ```
    - **Time Estimate:** 3 hours

- [ ] **Story V-1.2.2:** As a developer, I want integration tests for GetDailyContent error scenarios so that I can verify correct error handling.
    - **AC 1:** Test: `GetDailyContent_BlobNotFound_Returns404`
        - Do NOT seed blob to Azurite (empty storage)
        - Call `GET /api/GetDailyContent`
        - Assert HTTP 404 status
        - Assert response body contains "Content not yet published" message
    - **AC 2:** Test: `GetDailyContent_InvalidConnectionString_Returns500`
        - Configure function with invalid connection string (override in test factory)
        - Call endpoint
        - Assert HTTP 500 status
        - Assert response body contains "Storage configuration error" message
    - **AC 3:** Test: `GetDailyContent_MalformedJson_Returns500`
        - Seed invalid-content.json (malformed JSON) to Azurite
        - Call endpoint
        - Assert HTTP 500 status
        - Assert response indicates deserialization failure
    - **AC 4:** All error tests verify correct error logging (check logs if accessible in test environment)
    - **Time Estimate:** 3 hours

- [ ] **Story V-1.2.3:** As a developer, I want integration tests for GetDailyContent retry scenarios so that I can verify Polly policy execution.
    - **AC 1:** Test: `GetDailyContent_TransientFailure_RetriesAndSucceeds`
        - Use fault injection or Azurite restart to simulate transient failure
        - Verify Polly retries 3 times (check logs or custom telemetry)
        - Assert eventual success (HTTP 200)
        - Note: This may require custom middleware or Polly callback to verify retry attempts
    - **AC 2:** Test: `GetDailyContent_PermanentFailure_RetriesExhausted_Returns500`
        - Simulate permanent failure (e.g., Azurite stopped, not restarted)
        - Verify 3 retry attempts
        - Assert HTTP 500 after retries exhausted
    - **AC 3:** Document retry testing limitations: "Integration tests verify retry logic executes; timing validation deferred to load testing"
    - **Edge Cases:**
        - Azurite restart timing: May need delays or polling to simulate transient failure accurately
        - Retry timing verification: Integration tests focus on functional correctness, not precise timing
    - **Time Estimate:** 4 hours

---

## Feature V-1.03: ReactToContent Integration Tests
*Comprehensive HTTP-layer tests for reaction submission endpoint covering creation, increment, concurrency, and failure scenarios.*

- [ ] **Story V-1.3.1:** As a developer, I want integration tests for ReactToContent success scenarios so that I can verify correct reaction persistence.
    - **AC 1:** Create `src/Aurora.Api.IntegrationTests/ReactToContentTests.cs`
    - **AC 2:** Test: `ReactToContent_NewReaction_CreatesEntityWithCount1`
        - Do NOT seed reaction entity (fresh start)
        - Call `POST /api/articles/{id}/react`
        - Assert HTTP 200 status
        - Assert response body contains count: 1
        - Verify entity exists in Table Storage with count=1
    - **AC 3:** Test: `ReactToContent_ExistingReaction_IncrementsCount`
        - Seed reaction entity with count=10
        - Call `POST /api/articles/{id}/react`
        - Assert HTTP 200 status
        - Assert response body contains count: 11
        - Verify Table Storage entity updated to count=11
    - **AC 4:** Test: `ReactToContent_MultipleSequentialCalls_IncrementsCorrectly`
        - Seed reaction entity with count=5
        - Call endpoint 3 times sequentially
        - Assert final count=8
        - Verify all 3 responses show incrementing values (6, 7, 8)
    - **Time Estimate:** 3 hours

- [ ] **Story V-1.3.2:** As a developer, I want integration tests for ReactToContent concurrency scenarios so that I can verify atomic operations.
    - **AC 1:** Test: `ReactToContent_ConcurrentCalls_AllIncrementsApplied`
        - Seed reaction entity with count=0
        - Make 10 parallel calls using `Task.WhenAll()`
        - Assert final count=10 in Table Storage
        - Verify no increments were lost due to race conditions
    - **AC 2:** Note: Azure Table Storage uses optimistic concurrency (ETags); test verifies Azure SDK handles retries correctly
    - **AC 3:** Document concurrency testing limitations: "Integration tests verify functional correctness; load testing with 100+ concurrent users deferred to separate performance milestone"
    - **Edge Cases:**
        - Concurrency conflicts: Azure SDK retries automatically; test verifies all increments eventually succeed
        - Timing variations: Test may take 2-5 seconds due to retries; use generous timeout
    - **Time Estimate:** 3 hours

- [ ] **Story V-1.3.3:** As a developer, I want integration tests for ReactToContent error scenarios so that I can verify correct error handling.
    - **AC 1:** Test: `ReactToContent_InvalidArticleId_Returns400`
        - Call endpoint with malformed ID (e.g., empty string, invalid format)
        - Assert HTTP 400 status (if validation implemented) or HTTP 500 (if not)
        - Document current behavior
    - **AC 2:** Test: `ReactToContent_TableStorageUnavailable_Returns500`
        - Stop Azurite or configure invalid connection string
        - Call endpoint
        - Assert HTTP 500 status
        - Assert error message indicates storage failure
    - **AC 3:** Test: `ReactToContent_MissingArticleId_Returns400Or404`
        - Call endpoint without article ID in route (if possible)
        - Assert appropriate error response
    - **Time Estimate:** 2 hours

---

## Feature V-1.05: Testing Documentation
*Comprehensive documentation for integration testing architecture, setup, and best practices.*

- [ ] **Story V-1.5.1:** As a developer, I want comprehensive integration testing documentation so that I can understand the test infrastructure and add new tests confidently.
    - **AC 1:** Create `docs/testing/INTEGRATION_TESTING.md` with sections:
        - **Overview:** Purpose and scope of integration tests vs unit tests
        - **Architecture:** How Aurora.Api.IntegrationTests hosts functions in-process
        - **Prerequisites:** Azurite installation, Node.js/npm, .NET 9 SDK
        - **Running Tests Locally:** Step-by-step commands with screenshots/examples
        - **Running Tests in CI:** How GitHub Actions workflow executes tests
        - **Adding New Tests:** Template and best practices for new endpoint tests
        - **Test Data Management:** Using TestDataSeeder, fixtures, cleanup strategies
        - **Troubleshooting:** Common errors and solutions
    - **AC 2:** Include architecture diagram showing test flow:
        ```
        Integration Test → HttpClient → TestWebApplicationFactory → Aurora.Api Function → Azurite
        ```
    - **AC 3:** Document what integration tests DO cover vs DO NOT cover:
        - ✅ DO: HTTP request/response, middleware, actual storage operations, retry logic
        - ❌ DO NOT: Load testing, performance benchmarks (separate milestone), security testing
    - **AC 4:** Provide example test with detailed annotations (similar to CT-1.6.3 pattern)
    - **AC 5:** Link to official Microsoft docs for `WebApplicationFactory` and Azurite
    - **Time Estimate:** 3 hours
