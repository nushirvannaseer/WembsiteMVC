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
    if not exists (select* from Users where username=@UName)
    begin
        insert into Users values(@UName,@FName,@LName,@Email,@Pass)
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
    if exists (select* from Users where username=@UName)
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

    if exists (select * from Users where username=@username)
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
    if exists( select* from UserContent where contentID=@ContID)
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

    @Out int OUTPUT
as
begin
    declare @contID int,
        @FileT varchar(20),
        @Dat datetime

    set @FileT='text'
    set @Dat=GETDATE()
    select @contID=count(*)+1 from UserContent

    set @Out=0
    if not exists (select * from UserContent where contentID=@contID)
    begin
        insert into UserContent values(@contID,@UName,@privacy,@Dat,@FileT, @RData, 0)
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
    if exists(select* from UserContent where contentID=@contID)
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
    if not exists (select * from following where usernameA=@UserA and usernameB=@UserB)
    BEGIN
        insert into following values(@UserA,@UserB)
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

    if exists (select * from following where usernameA=@UserA and usernameB=@UserB)
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
	select * from Users where username=@username
end

go

--procedure to display all users
drop procedure AllUsers
go
create procedure AllUsers
as
	begin
		select * from Users
	end
go

--return posts on homepage of user
drop PROCEDURE homepage
GO
create PROCEDURE homepage
    @userID varchar(30)
AS
    BEGIN
       select * from UserContent as UC
        where username=(	
				select usernameA as peopleIFollow
				from following 
				where [following].usernameB=@userID
			) and privacy!='Only Me'
        order by DateCreation DESC
    end
go


create procedure [likePost]
	@likedBy varchar(30),
	@contentID int,
	@postOwner varchar(30)
as
	begin
		if not exists (select * from likes 
						where likedBy=@likedBy and contentID=@contentID
						)
			begin
				insert into likes
				values(@contentID, @likedBy, @postOwner)
				update UserContent
				set UserContent.likes=UserContent.likes+1
				where contentID=@contentID
			end
	end

go
create procedure unlikePost
    @unlikedBy varchar(30),
    @contentID int,
    @postOwner varchar(30)
as 
    BEGIN   
        delete from likes where contentID=@contentID and likedBy=@unlikedBy
        update UserContent
        set UserContent.likes=UserContent.likes-1
        where contentID=@contentID
    END
go


create procedure GetLikeList
	@contentID int,
	@postOwner varchar(30)
as
begin
	select * from likes
	where contentID=@contentID and postOwner=@postOwner
end
go