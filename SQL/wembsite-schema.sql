create database wembsite;
go
use wembsite;

go

create table Users
(
    username varchar(30) not null,
    firstname varchar(20) not null,
    lastname varchar(20) not null,
    email varchar(30) not null,
    upassword varchar(30) not null,

    primary key(username),
)
go

create table following
(
    usernameA varchar(30),
    usernameB varchar(30),
);

go

ALTER table following
add CONSTRAINT UA_FK
    foreign key (usernameA) references Users(username) on delete set null on update cascade;

go
ALTER table following
add CONSTRAINT UB_FK
    foreign key (usernameB) references Users(username) --on delete set null on update cascade;
go

drop table UserContent
go

create table UserContent
(
    contentID int primary key,
    username varchar(30),
    privacy varchar(20) not null,
    DateCreation datetime  not null,
    FileType varchar(20) not null,
    RawData varchar(8000) not null,
	likes int

    foreign key (username) references Users(username) on delete set null on update cascade,

);
go

create table likes
(
    contentID int,
    likedBy varchar(30),

	foreign key (likedBy) references Users(username) on delete set null on update cascade,
    foreign key (contentID) references UserContent(contentID) on delete set null on update no action
);
go

create table comments
(
    commentID int primary key,
	contentID int,
	commentedBy varchar(30),
	commentText varchar(3000),

	foreign key (commentedBy) references Users(username) on delete no action on update no action,
    foreign key (contentID) references UserContent(contentID)  on delete no action on update no action
);
go

create table followRequests
(
	sender varchar (30),
	receiver varchar (30)
);