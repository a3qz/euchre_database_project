create table Partner(
    gameid varchar(37), 
    partner1netid varchar(8),
    partner2netid varchar(8),
    partner1score int,
    partner2score int,
    teamnumber int,

    foreign key (gameid) references Game(gameid),
    foreign key (partner1netid) references Player(netid),
    foreign key (partner2netid) references Player(netid),
    primary key (gameid, partner1netid, partner2netid)
);
