using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<TutorContext>(options =>
    options.UseInMemoryDatabase("TutoresInMemory"));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

var tutores = app.MapGroup("/tutores");

tutores.MapGet("/", async (TutorContext context)
    => await context.Tutores.ToListAsync());

tutores.MapGet("/{id}", async (int id, TutorContext context)
    => await context.Tutores.FindAsync(id));

tutores.MapPost("/", async (Tutor tutor, TutorContext context) =>
{
    context.Add(tutor);
    await context.SaveChangesAsync();
});

tutores.MapPut("/", async (Tutor tutor, TutorContext context) =>
{
    context.Update(tutor);
    await context.SaveChangesAsync();
});

tutores.MapDelete("/{id}", async (int id, TutorContext context) =>
{
    var tutor = await context.Tutores.FindAsync(id);

    if (tutor is null) return;

    context.Remove(tutor);
    await context.SaveChangesAsync();
});

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.Run();

public record Tutor(int Id, string Name, string Email, string Senha);

public class TutorContext : DbContext
{
    public TutorContext(DbContextOptions options) : base(options) { }

    public DbSet<Tutor> Tutores => Set<Tutor>();
}
