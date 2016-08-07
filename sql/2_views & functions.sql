DROP FUNCTION IF EXISTS core.get_total_view(bigint);
CREATE OR REPLACE FUNCTION core.get_total_votes(bigint) RETURNS integer as $$
        BEGIN
                RETURN (SELECT COUNT(*) FROM core.votes WHERE contestant_id = $1);
        END;
$$ LANGUAGE plpgsql;


DROP VIEW IF EXISTS core.contestant_view;
CREATE OR REPLACE VIEW  core.contestant_view AS (
    SELECT  u.id,
            u.user_name as UserName,
            u.password,
            u.full_name as FullName,
            cd.age,
            cd.suburb,
            cd.parents_name as ParentsName,
            cd.contact,
            cd.email,
            cd.about_me as AboutMe,
            core.get_total_votes(cd.user_id) as TotalVotes
    FROM core.users as u 
        LEFT OUTER JOIN core.contestant_details cd ON cd.user_id = u.id
        WHERE u.Role = 'Contestant'
);
