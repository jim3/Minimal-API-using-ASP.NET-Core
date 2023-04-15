using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// Sample parts database
var parts = new[] {
    new { Name = "screws", Type = "wood", Quantity = 100, Price = 0.12m },
    new { Name = "nuts", Type = "hex", Quantity = 150, Price = 0.18m },
    new { Name = "bolts", Type = "eye", Quantity = 75, Price = 0.15m },
    new { Name = "washers", Type = "flat", Quantity = 200, Price = 0.12m },
    new { Name = "bolts", Type = "carriage", Quantity = 25, Price = 0.19m }
};

// Get all parts
app.MapGet("/api/parts", () => {
    return Results.Json(parts);
});

// Create new part
app.MapPost("/api/parts", (string partname, string parttype, int quantity, decimal price) => {
    var newPart = new {
        Name = partname,
        Type = parttype,
        Quantity = quantity,
        Price = price
    };

    return Results.Json(newPart);
});

// Get a specific part
app.MapGet("/api/parts/{partname}", (string partname) => {
    var part = parts.FirstOrDefault(p => p.Name == partname);     // Find the part

    if (part == null) {
        return Results.NotFound();
    }

    return Results.Json(part);
});

// Update a specific part
app.MapPut("/api/parts/{partname}", (string partname, string parttype, int quantity, decimal price) => {
    var part = parts.FirstOrDefault(p => p.Name == partname); // Find the part

    if (part == null) {
        return Results.NotFound();
    }

    // Create a new anonymous object with the updated values
    var updatedPart = new {
        Name = partname,
        Type = parttype,
        Quantity = quantity,
        Price = price
    };

    // Update part
    parts = parts.Where(p => p.Name != partname).ToArray();
    parts.Append(updatedPart);

    return Results.Json(updatedPart);
});

// Delete a specific part
app.MapDelete("/api/parts/{partname}", (string partname) => {
    var part = parts.FirstOrDefault(p => p.Name == partname);   // Find the part

    if (part == null) {
        return Results.NotFound();
    }

    parts = parts.Where(p => p.Name != partname).ToArray(); // Remove the part from the array
    return Results.Ok();
});

app.Run();
