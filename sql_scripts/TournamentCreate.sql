create table Tournament(
    tournamentid varchar(37),
    gameid varchar(37),
    roundnumber int,
    foreign key (gameid) references Game(gameid),
    primary key (tournamentid, gameid)
);