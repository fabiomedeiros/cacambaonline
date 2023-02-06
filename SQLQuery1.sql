select * from Notificacoes

select * from Notificacoes


Delete LogCTR
Delete CTR_antigo
Delete CTR_novo
Delete CTR

DBCC CHECKIDENT('LogCTR', RESEED, 0)
DBCC CHECKIDENT('CTR_antigo', RESEED, 0)
DBCC CHECKIDENT('CTR_novo', RESEED, 0)
DBCC CHECKIDENT('CTR', RESEED, 0)


Delete LogNotificacoes
Delete Notificacoes_Antigas
Delete Notificacoes_Novas
Delete Notificacoes

DBCC CHECKIDENT('LogNotificacoes', RESEED, 0)
DBCC CHECKIDENT('Notificacoes_Antigas', RESEED, 0)
DBCC CHECKIDENT('Notificacoes_Novas', RESEED, 0)
DBCC CHECKIDENT('Notificacoes', RESEED, 0)



