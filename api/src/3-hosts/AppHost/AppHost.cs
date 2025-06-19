using SplitTheBill.AppHost.Constants;

var builder = DistributedApplication.CreateBuilder(args);

#region Database


#endregion

#region API

var api = builder.AddProject<Projects.Api>(Resources.Api).WithHttpHealthCheck("/health");

#endregion

#region Frontend


#endregion

builder.Build().Run();
