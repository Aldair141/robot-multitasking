select * from tb_cliente
where convert(varchar(10), fecre, 103) = '18/05/2021' and docuidentid = '6'
go

delete from tb_cliente
where convert(varchar(10), fecre, 103) = '18/05/2021' and docuidentid = '6'
go

create table tb_task_scheduler
(
	TaskID int primary key,
	TaskName varchar(100),
	TaskDate datetime,
	TaskHour time,
	TaskLastDate datetime,
	TaskStatus bit
)
go

insert into tb_task_scheduler values
(
	1,
	'SUNAT_DATA',
	GETDATE(),
	'11:30:00',
	GETDATE(),
	1
)
go

insert into tb_task_scheduler values
(
	2,
	'IMPRIMIR_TEXTO',
	GETDATE(),
	'11:30:00',
	GETDATE(),
	1
)

create procedure tb_task_scheduler_SELECT
@hour time
as
	select * from tb_task_scheduler
	where convert(varchar(5),TaskHour, 108) = convert(varchar(5),@hour,108)
	and TaskStatus = 1
go

update tb_task_scheduler set TaskHour = '15:43', TaskStatus = 1
go

exec tb_task_scheduler_SELECT '15:43'
go