CREATE TABLE IF NOT EXISTS Card(
                                   ID INT PRIMARY KEY     NOT NULL,
                                   NAME           TEXT    NOT NULL,
                                   DAMAGE            INT     NOT NULL
);

CREATE TABLE IF NOT EXISTS "User"(
                                   ID INT PRIMARY KEY GENERATED ALWAYS AS IDENTITY ,
                                   Username TEXT NOT NULL,
                                   Password TEXT NOT NULL
);