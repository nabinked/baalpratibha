    DROP SCHEMA IF EXISTS core CASCADE;
    CREATE SCHEMA core;

    DROP TABLE IF EXISTS core.users;
    CREATE TABLE IF NOT EXISTS core.users (
        id 					bigserial PRIMARY KEY,
        user_name			varchar(500) UNIQUE NOT NULL,
        password    		varchar NOT NULL,
        full_name           varchar(500),
        role             	varchar(50)
        );
    --One Admin
    INSERT INTO core.users (user_name, password, full_name, role) VALUES('admin','bpadmin','Baal Pratibha Admin', 'Admin');


    DROP TABLE IF EXISTS core.contestant_details;
    CREATE TABLE IF NOT EXISTS core.contestant_details (
        user_id			        bigint UNIQUE REFERENCES core.users ON DELETE CASCADE,
        age                     int NOT NULL,
        suburb                  varchar(150) NOT NULL,
        parents_name            varchar(500) NOT NULL,
        contact                 varchar(100) NOT NULL,
        email                   varchar(100) NOT NULL,
        performance_video_url   text,
        about_me                text NOT NULL

        );

    DROP TABLE IF EXISTS core.votes;
    CREATE TABLE IF NOT EXISTS core.votes(
        id                  bigserial PRIMARY KEY,
        contestant_id       int REFERENCES core.users ON DELETE CASCADE,
        voter_name          varchar(250) NOT NULL,
        voter_email         varchar(250) UNIQUE NOT NULL 
    );

    DROP TABLE IF EXISTS core.shares;
    CREATE TABLE IF NOT EXISTS core.shares(
        contestant_id       int UNIQUE REFERENCES core.users ON DELETE CASCADE,
        total_shares        int 

    );
