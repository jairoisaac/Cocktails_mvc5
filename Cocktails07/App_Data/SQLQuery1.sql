CREATE TABLE [dbo].[CocktailProcess] (
    [CocktailId] BIGINT NOT NULL,
    [Id]         BIGINT NOT NULL,
    CONSTRAINT [PK_CocktailProcess_1] PRIMARY KEY CLUSTERED ([CocktailId] ASC, [Id] ASC),
    CONSTRAINT [FK_CocktailProcess_Process] FOREIGN KEY ([Id]) REFERENCES [dbo].[Process] ([Id]),
    CONSTRAINT [FK_CocktailProcess_CockTail] FOREIGN KEY ([CocktailId]) REFERENCES [dbo].[CockTail] ([CocktailId]) ON DELETE CASCADE
);
