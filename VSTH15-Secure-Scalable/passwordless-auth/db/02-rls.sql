/*
    Create tables to store data and user level
*/
drop security policy [security_level_policy];
drop function [dbo].[check_security_level];

drop table if exists dbo.users_security_levels;
create table dbo.users_security_levels
(
    [user] sysname not null primary key,
    [level] int not null
)
go
insert into 
    dbo.users_security_levels 
values
    ('<your account>', 0),
    ('<managed-identity-id>', 1) -- you can get it via: select suser_sname() 
go

drop table if exists dbo.sample_data;
create table dbo.sample_data
(
    id int identity not null primary key,
    some_data nvarchar(100) not null, 
    security_level int 
)
go
insert into dbo.sample_data
    (some_data, security_level)
values
    ('Data for none', -1),
    ('Data for few', 0),
    ('Data for some', 1),
    ('Data for all', 2)
go

/*
    Create function to check if user as the right level to
    access the requested data
*/
create function dbo.check_security_level(@level as int)
returns table
with schemabinding
as
return
    select 1 as authorized from dbo.users_security_levels 
    where [user] = suser_name() and [level] <= @level and [level] >= 0
go

/*
    Use the created function to authorize access to the dbo.sample_data table,
    but set the row-level security to off for now
*/
create security policy [security_level_policy]
add filter predicate dbo.check_security_level([security_level])
on dbo.sample_data
with (state = off)
go

/*
    Verify that you can still access all data
*/
select * from dbo.sample_data
go

/*
    Enable row-level security
*/
alter security policy [security_level_policy]
with (state = on)
go

/*
    You can only see the data compatible with your level
*/
select * from dbo.sample_data
go

/*
    Impersonate the application user
*/
execute as user = '<managed-identity-name>'
go

/*
    Only the data allowed by the application level is visibile
*/
select * from dbo.sample_data
go

/*
    Back to original user
*/
revert
go

select suser_sname()
go

select * from dbo.sample_data
go

/*
    Create a new user without any level specification
*/
create user [sample_user] without login
go

alter role [db_datareader] add member [sample_user]
go

execute as user = 'sample_user'
go

/* 
    Nothing is visibile
*/
select * from dbo.sample_data
go

/*
    Revert and cleanup
*/
revert
go

drop user [sample_user]
go
