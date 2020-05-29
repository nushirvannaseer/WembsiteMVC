--SQL methods
use wembsite
go

--procedure to register new user
drop procedure NewUser
go
create procedure NewUser
    @UName varchar(30),
    @FName varchar(20),
    @LName varchar(20),
    @Email varchar(30),
    @Pass varchar(30),

    @Out int OUTPUT
as
begin
    set @Out=0
    if not exists (select*
    from Users
    where username=@UName)
    begin
        insert into Users
        values(@UName, @FName, @LName, @Email, @Pass)
        set @Out=1
    end
end

go

--update password
drop procedure UpdatePassword
go
create procedure UpdatePassword
    @NewPass varchar(30),
    @UName varchar(30),

    @Out int OUTPUT
as
begin
    set @Out=0
    if exists (select*
    from Users
    where username=@UName)
    BEGIN
        update Users
        Set upassword=@NewPass
        where username=@UName
        set @Out=1
    end
end

go

--delete user
drop procedure DeleteUser
go
create procedure DeleteUser
    @username varchar(30),

    @Out int OUTPUT
as
begin
    set @Out=0

    if exists (select *
    from Users
    where username=@username)
    begin
        Delete from Users
        where username=@username
        set @Out=1
    end
end

go

--Delete Post
drop procedure DeletePost
go
create procedure DeletePost
    @ContID int,

    @Out int OUTPUT
as
begin
    set @Out=0
    if exists( select*
    from UserContent
    where contentID=@ContID)
    begin
        Delete From UserContent
        where contentID=@ContID
        set @Out=1
    end
end

go

--Add post
drop procedure AddPost
go
create procedure AddPost
    @UName varchar(30),
    @privacy varchar(20),
    @RData varchar(8000),
    @filepath varchar(300),
    @FileT varchar(20),

    @Out int OUTPUT
as
begin
    declare @contID int,
        @Dat datetime

    set @Dat=GETDATE()
    select @contID=max(contentID)+1
    from UserContent

    set @Out=0
    if not exists (select *
    from UserContent
    where contentID=@contID)
    begin
        insert into UserContent
        values(@contID, @UName, @privacy, @Dat, @FileT, @RData, 0, @filepath)
        set @Out=1
    end
end

go

--Edit post
drop procedure EditPost
go
create procedure EditPost
    @contID int,
    @RawData varchar(8000),
    @privacy varchar(20),
    @Out int OUTPUT
as
begin
    set @Out=0
    if exists(select*
    from UserContent
    where contentID=@contID)
    begin
        update UserContent
        set RawData=@RawData,
        privacy=@privacy
        where contentID=@contID
        set @Out=1
    end
end

go

--new follower
drop procedure NewFollower
go
create procedure NewFollower
    @UserA varchar(30),
    @UserB varchar(30),

    @Out int OUTPUT
as
begin
    set @Out=0
    if not exists (select *
    from following
    where usernameA=@UserA and usernameB=@UserB)
    BEGIN
        insert into following
        values(@UserA, @UserB)
        set @Out=1
    end
end

go

--remove follower
drop procedure DeleteFollower
go
create procedure DeleteFollower
    @UserA varchar(30),
    @UserB varchar(30),

    @Out int OUTPUT
as
begin
    set @Out=0

    if exists (select *
    from following
    where usernameA=@UserA and usernameB=@UserB)
    BEGIN
        delete from following
        where usernameA=@UserA and usernameB=@UserB
        set @Out=1
    end
end
GO

--get User
drop PROCEDURE getUser
go
create procedure getUser
    @username varchar(30)
as
begin
    select *
    from Users
    where username=@username
end

go

--procedure to display all users
drop procedure AllUsers
go
create procedure AllUsers
as
begin
    select *
    from Users
end
go

--return posts on homepage of user
drop PROCEDURE homepage
GO
create PROCEDURE homepage
    @userID varchar(30)
AS
BEGIN
    select *
    from UserContent as UC
    where username in (	
				select usernameA as peopleIFollow
        from following
        where [following].usernameB=@userID
			) and privacy!='Only Me'
    order by DateCreation DESC
end
go


create procedure [likePost]
    @likedBy varchar(30),
    @contentID int
as
begin
    if not exists (select *
    from likes
    where likedBy=@likedBy and contentID=@contentID
						)
			begin
        insert into likes
        values(@contentID, @likedBy)
        update UserContent
				set UserContent.likes=UserContent.likes+1
				where contentID=@contentID
    end
end

go
create procedure unlikePost
    @unlikedBy varchar(30),
    @contentID int
as
BEGIN
    delete from likes where contentID=@contentID and likedBy=@unlikedBy
    update UserContent
        set UserContent.likes=UserContent.likes-1
        where contentID=@contentID
END
go

drop procedure getNonSessionUserPosts
GO
create procedure getNonSessionUserPosts
    @username varchar(30),
    @sessionUserIsFollowing int

as
begin
    if @sessionUserIsFollowing=1
			begin
        select *
        from UserContent
        where username=@username and privacy!='Only Me'
    end
		else
			begin
        select *
        from UserContent
        where username=@username and privacy='Public'
        order by DateCreation DESC
    end
end
go

drop PROCEDURE GetLikeList
go
create procedure GetLikeList
    @contentID int
as
begin
    select *
    from likes
    where contentID=@contentID
end
go

drop PROCEDURE Search
go
create procedure Search
    @searchText varchar(100)

as
begin
    select username
    from [Users]
    where username like '%'+@searchText+'%'
end
go

drop procedure addComment
go
create PROCEDURE addComment
    @postID int,
    @userIDD varchar(30),
    @text varchar(3000),

    @out int OUTPUT
AS
BEGIN
    set @out=0
    declare @commmentID INT
    select @commmentID=count(*)+1
    from comments

    insert into comments
    values(@commmentID, @postID, @userIDD, @text)
    set @out=1
end
go

create procedure getCommentsOfAPost
    @contentID int
as
begin
    select *
    from comments
    where contentID=@contentID
end
go

drop PROCEDURE sendFollowReq
go
create PROCEDURE sendFollowReq
    @userA varchar(30),
    @userB VARCHAR(30)
as
BEGIN
    if not exists (
        select* 
        from followRequests
        where sender=@userA and receiver=@userB)
    begin
        insert into followRequests values(@userA, @userB)
    end
end
