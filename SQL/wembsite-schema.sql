create database wembsite;

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


create table UserContent
(
    contentID int primary key,
    username varchar(30),
    privacy varchar(20) not null,
    DateCreation datetime  not null,
    FileType varchar(20) not null,
    RawData varchar(8000) not null,

    foreign key (username) references Users(username) on delete set null on update cascade

);
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

create table shared
(
    contentID int,
    username varchar(30),

    foreign key (username) references Users(username) on delete set null on update cascade,
    foreign key (contentID) references UserContent(contentID) 
);

create table followRequests
(
	sender varchar (30),
	receiver varchar (30)
);