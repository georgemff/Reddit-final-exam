using Microsoft.EntityFrameworkCore;
using Reddit.Repositories;
using Reddit.Models;

namespace Reddit.UnitTests;

public class PagedListTests
{
       
    private ApplicationDbContext CreateContext()
    {
        var dbName = Guid.NewGuid().ToString();
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: dbName).Options;

        var context = new ApplicationDbContext(options);

        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();

        //Post[] posts = [];
        //for (var i = 1; i <= 10; i++)
        //{
        //    posts.Append(new Post { Id = i, Title = "Post", Content = "Content", AuthorId = 1, Comments = new List<Comment>(), Downvotes = 1, Upvotes = 10});

        //}

        context.Posts.AddRange(
            new Post { Id = 1, Title = "Post", Content = "Content", AuthorId = 1, Comments = new List<Comment>(), Downvotes = 1, Upvotes = 10 },
            new Post { Id = 2, Title = "Post", Content = "Content", AuthorId = 1, Comments = new List<Comment>(), Downvotes = 1, Upvotes = 10 },
            new Post { Id = 3, Title = "Post", Content = "Content", AuthorId = 1, Comments = new List<Comment>(), Downvotes = 1, Upvotes = 10 },
            new Post { Id = 4, Title = "Post", Content = "Content", AuthorId = 1, Comments = new List<Comment>(), Downvotes = 1, Upvotes = 10 },
            new Post { Id = 5, Title = "Post", Content = "Content", AuthorId = 1, Comments = new List<Comment>(), Downvotes = 1, Upvotes = 10 },
            new Post { Id = 6, Title = "Post", Content = "Content", AuthorId = 1, Comments = new List<Comment>(), Downvotes = 1, Upvotes = 10 },
            new Post { Id = 7, Title = "Post", Content = "Content", AuthorId = 1, Comments = new List<Comment>(), Downvotes = 1, Upvotes = 10 },
            new Post { Id = 8, Title = "Post", Content = "Content", AuthorId = 1, Comments = new List<Comment>(), Downvotes = 1, Upvotes = 10 },
            new Post { Id = 9, Title = "Post", Content = "Content", AuthorId = 1, Comments = new List<Comment>(), Downvotes = 1, Upvotes = 10 },
            new Post { Id = 10, Title = "Post", Content = "Content", AuthorId = 1, Comments = new List<Comment>(), Downvotes = 1, Upvotes = 10 }
            );

        context.SaveChanges();

        return context;
    }
      
        
    [Fact]
    public async Task GetPosts_RetursFirstPage()
    {
        //should return 5 items
        using var context = CreateContext();
        Assert.NotNull(context);
        var items = context.Posts.AsQueryable();
        var list = await PagedList<Post>.CreateAsync(items, 1, 5);
        Assert.Equal(10, list.TotalCount);
        Assert.Equal(5, list.Items.Count);
        Assert.Equal(1, list.Page);
        Assert.Equal(5, list.PageSize);
        Assert.True(list.HasNextPage);
        Assert.False(list.HasPreviousPage);
    }

    [Fact]
    public async Task GetPosts_RetursLastPage()
    {
        //should return 5 items
        using var context = CreateContext();
        Assert.NotNull(context);
        var items = context.Posts.AsQueryable();
        var list = await PagedList<Post>.CreateAsync(items, 2, 5);
        Assert.Equal(10, list.TotalCount);
        Assert.Equal(5, list.Items.Count);
        Assert.False(list.HasNextPage);
        Assert.True(list.HasPreviousPage);
    }

    [Fact]
    public async Task GetPosts_LargerPageSize()
    {
        //should return 5 items
        using var context = CreateContext();
        Assert.NotNull(context);
        var items = context.Posts.AsQueryable();
        var list = await PagedList<Post>.CreateAsync(items, 1, 15);
        Assert.Equal(10, list.TotalCount);
        Assert.Equal(10, list.Items.Count);
        Assert.False(list.HasNextPage);
        Assert.False(list.HasPreviousPage);
    }
    
    [Fact]
    public async Task GetPosts_LargerPageNumber()
    {
        //should return 5 items
        using var context = CreateContext();
        Assert.NotNull(context);
        var items = context.Posts.AsQueryable();
        var list = await PagedList<Post>.CreateAsync(items, 3, 5);
        Assert.Equal(10, list.TotalCount);
        Assert.Equal(0, list.Items.Count);
        Assert.False(list.HasNextPage);
        Assert.True(list.HasPreviousPage);
    }
}
