create table Tournament(
    tournamentid int,
    gameid int,
    roundnumber int,
    foreign key (gameid) references Game(gameid),
    primary key (tournamentid, gameid)
);
