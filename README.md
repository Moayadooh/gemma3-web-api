
Run the app:
dotnet run
or
dotnet watch run

Test the API:
curl -X POST "http://localhost:5215/generate" \
-H "Content-Type: application/json" \
-d '{"prompt": "Hi"}'
