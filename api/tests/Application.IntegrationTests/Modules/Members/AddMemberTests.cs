using Microsoft.EntityFrameworkCore;
using Shouldly;
using SplitTheBill.Application.IntegrationTests.Common;
using SplitTheBill.Application.Tests.Shared.TestData;
using SplitTheBill.Domain.Models.Members;

namespace SplitTheBill.Application.IntegrationTests.Modules.Members;

internal sealed class AddMemberTests : ApplicationTestBase
{
    // these tests currently only verify on the database-level constraints
    // when other operations such as creating and updating members are implemented,
    // tests which verify the validation and application-level constraints should be added

    // Username:
    // 3-32 characters
    // only a-zA-Z0-9._ 
    // case-insensitive unique
    // start with letter or _

    // UserId
    // case-insensitive unique

    [Test]
    public async Task Username_TooLong_Fails()
    {
        var member = new Member
        {
            Id = Guid.NewGuid(),
            Name = "name",
            Username = TestUtilities.GenerateString(33),
        };
        var exception = await Should.ThrowAsync<DbUpdateException>(async () => { await Application.AddAsync(member); });
        var innerException = exception.InnerException.ShouldNotBeNull();
        innerException.Message.ShouldBe("22001: value too long for type character varying(32)");
    }

    [Test]
    [Arguments(15)]
    [Arguments(32)]
    public async Task Username_ValidLength_Inserts(int length)
    {
        var id = Guid.NewGuid();
        var username = TestUtilities.GenerateString(length);
        var member = new Member
        {
            Id = id,
            Name = "name",
            Username = username,
        };
        await Application.AddAsync(member);
        var persistedMember = await Application.FindAsync<Member>(m => m.Id == id);
        persistedMember.ShouldNotBeNull();
        persistedMember.Username.ShouldBe(username);
    }

    [Test]
    [Arguments("username", "username")]
    [Arguments("username", "Username")]
    [Arguments("username", "ùsername")]
    public async Task Username_NotUnique_Fails(string username1, string username2)
    {
        await Application.AddAsync(new Member
            {
                Id = Guid.NewGuid(),
                Name = "name",
                Username = username1,
            }
        );
        var exception = await Should.ThrowAsync<DbUpdateException>(async () => await Application
            .AddAsync(new Member
            {
                Id = Guid.NewGuid(),
                Name = "other name",
                Username = username2,
            }));
        var innerException = exception.InnerException.ShouldNotBeNull();
        innerException.Message.ShouldStartWith(
            "23505: duplicate key value violates unique constraint \"IX_Members_Username\"");
    }

    [Test]
    public async Task Username_Unique_Inserts()
    {
        var id1 = Guid.NewGuid();
        var id2 = Guid.NewGuid();
        await Application.AddAsync(new Member
        {
            Id = id1,
            Name = "name",
            Username = "username1"
        });
        await Application.AddAsync(new Member
        {
            Id = id2,
            Name = "name",
            Username = "username2"
        });
        var member1 = await Application.FindAsync<Member>(m => m.Id == id1);
        member1.ShouldNotBeNull();
        member1.Username.ShouldBe("username1");
        var member2 = await Application.FindAsync<Member>(m => m.Id == id2);
        member2.ShouldNotBeNull();
        member2.Username.ShouldBe("username2");
    }

    [Test]
    [Arguments("userid", "userid")]
    [Arguments("userid", "UserId")]
    [Arguments("userid", "ùserid")]
    public async Task UserId_NotUnique_Fails(string userId1, string userId2)
    {
        await Application.AddAsync(new Member
            {
                Id = Guid.NewGuid(),
                Name = "name",
                Username = "username1",
                UserId = userId1,
            }
        );
        var exception = await Should.ThrowAsync<DbUpdateException>(async () => await Application
            .AddAsync(new Member
            {
                Id = Guid.NewGuid(),
                Name = "other name",
                Username = "username2",
                UserId = userId2,
            }));
        var innerException = exception.InnerException.ShouldNotBeNull();
        innerException.Message.ShouldStartWith(
            "23505: duplicate key value violates unique constraint \"IX_Members_UserId\"");
    }

    [Test]
    public async Task UserId_Unique_Inserts()
    {
        var id1 = Guid.NewGuid();
        var id2 = Guid.NewGuid();
        await Application.AddAsync(new Member
        {
            Id = id1,
            Name = "name",
            Username = "username1",
            UserId = "id1",
        });
        await Application.AddAsync(new Member
        {
            Id = id2,
            Name = "name",
            Username = "username2",
            UserId = "id2",
        });
        var member1 = await Application.FindAsync<Member>(m => m.Id == id1);
        member1.ShouldNotBeNull();
        member1.Id.ShouldBe(id1);
        var member2 = await Application.FindAsync<Member>(m => m.Id == id2);
        member2.ShouldNotBeNull();
        member2.Id.ShouldBe(id2);
    }

    [Test]
    [Arguments(null)]
    [Arguments("userid")]
    public async Task ValidMember_Inserts(string? userId)
    {
        var id = Guid.NewGuid();
        var member = new Member
        {
            Id = id,
            Name = "name",
            Username = "username",
            UserId = userId
        };
        await Application.AddAsync(member);
        
        var persistedMember = await Application.FindAsync<Member>(m => m.Id == id);
        persistedMember.ShouldNotBeNull();
        persistedMember.Name.ShouldBe("name");
        persistedMember.Username.ShouldBe("username");
        persistedMember.UserId.ShouldBe(userId);
    }
}