alter proc GetUsers
as begin 
select id,[name],mailId,[role],isDeleted from tblUser;
end;


create proc GetUserById
(
@id int
)
as begin
select id,[name],mailId,[role],isDeleted from tblUser where id=@id;
end;

create proc DeleteUserById
(
@id int
)
as begin 
delete from tblUser where id=@id;
end

create proc InsertUser
(
@name nvarchar(50),
@role nvarchar(20),
@password nvarchar(200),
@mailId nvarchar(50)
)
as begin 
insert into tblUser (name,role,password,mailId) values(@name,@role,@password,@mailId);

select @@IDENTITY as 'Identity';

end;

create proc UpdateUser
(
@id int,
@name nvarchar(50),
@role nvarchar(20),
@password nvarchar(200),
@mailId nvarchar(50)
)
as begin
update tblUser set name=@name,password=@password,mailId=@mailId,role=@role where id=@id;
end







