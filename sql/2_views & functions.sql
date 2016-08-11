	DROP FUNCTION IF EXISTS core.get_total_votes(bigint) CASCADE;
	CREATE OR REPLACE FUNCTION core.get_total_votes(bigint) RETURNS integer as $$
		BEGIN
			RETURN (SELECT COUNT(*) FROM core.votes WHERE contestant_id = $1);
		END;
	$$ LANGUAGE plpgsql;

	DROP FUNCTION IF EXISTS core.get_total_shares(bigint);
	CREATE OR REPLACE FUNCTION core.get_total_shares(bigint) RETURNS integer as $$
		BEGIN
			RETURN (SELECT total_shares FROM core.shares WHERE contestant_id = $1);
		END;
	$$ LANGUAGE plpgsql;

	DROP FUNCTION IF EXISTS core.calc_rank_weight(bigint, bigint);
	CREATE OR REPLACE FUNCTION core.calc_rank_weight(bigint, bigint) RETURNS numeric(100, 2) as $$
		BEGIN
			RETURN (COALESCE($1,0) * 0.60) + (COALESCE($2,0) * 0.40);
		END;
	$$ LANGUAGE plpgsql;




	DROP VIEW IF EXISTS core.contestant_view;
	CREATE OR REPLACE VIEW  core.contestant_view AS (
		SELECT *,  dense_rank() OVER (ORDER BY core.calc_rank_weight(TotalVotes, TotalShares) desc) as Rank FROM 
	(
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
		    cd.performance_video_url as PerformanceVideoUrl,
		    core.get_total_votes(u.id) as TotalVotes,
		    core.get_total_shares(u.id) as TotalShares
	    FROM core.users as u 
		LEFT OUTER JOIN core.contestant_details cd ON cd.user_id = u.id
		WHERE u.Role = 'Contestant'
	) v1
	);
