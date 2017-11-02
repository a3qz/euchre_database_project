"""generate random euchre games"""
import random
from requests import Session
session = Session()
#session.head('https://prod-28.eastus.logic.azure.com:443/workflows/b85036662ad042deb191ebd51dda5394/triggers/manual/paths/invoke?api-version=2016-10-01&sp=%2Ftriggers%2Fmanual%2Frun&sv=1.0&sig=40vw--ScvCGpV5L5ETsPFsG-8kFfPqdGsw2bFVJxlvg')
print "{"
N = 300
PLAYERS = [
    ["rmichale", 90],
    ["smichale", 70],
    ["msemanek", 90],
    ["zspahr", 80],
    ["agood", 90],
    ["sjenkins", 60],
    ["pbrunts", 65],
    ["lfilipia", 70],
    ["alui", 70],
    ["jwang28", 55]
    ]
for i in range(N):
    random.shuffle(PLAYERS)
    s1 = 0
    s2 = 0
    while s1 < 10 and s2 < 10:
        t1pts = 0
        t2pts = 0
        for z in range(5):
            t1p1 = random.randint(0, PLAYERS[0][1])
            t1p2 = random.randint(0, PLAYERS[1][1])
            t2p1 = random.randint(0, PLAYERS[2][1])
            t2p2 = random.randint(0, PLAYERS[3][1])
            
            if t1p1 + t1p2 > t2p1+t2p2:
                t1pts += 1
            else:
                t2pts += 1
        if t1pts == 5:
            if random.randint(0, 100) < 2:
                s1 += 4
            else:
                s1 += 2
        elif t1pts >= 3:
            if random.randint(0, 100) < 13:
                s1 += 2
            else:
                s1 += 1
        elif t2pts == 5:
            if random.randint(0, 100) < 2:
                s2 += 4
            else:
                s2 += 2
        elif t2pts >= 3:
            if random.randint(0, 100) < 13:
                s2 += 2
            else:
                s2 += 1
    print '[', '{', '"players"', ":", map(lambda x: x[0], PLAYERS[:2]), ',', '"score:"', ":", "'"+ str(s1)+ "'",'}', ',',
    print '{', '"players"', ":", map(lambda x: x[0], PLAYERS[2:4]), ',', '"score:"', ":", "'"+str(s2)+ "'",'}',']',','
    #print '{', map(lambda x: x[0], PLAYERS[2:4]), s2, '}', ']'
    

    response = session.post(
        url='https://prod-28.eastus.logic.azure.com:443/workflows/b85036662ad042deb191ebd51dda5394/triggers/manual/paths/invoke?api-version=2016-10-01&sp=%2Ftriggers%2Fmanual%2Frun&sv=1.0&sig=40vw--ScvCGpV5L5ETsPFsG-8kFfPqdGsw2bFVJxlvg',
        json={
            'team1score': str(s1),
            'team2score': str(s2),
            'team1': {
                'partner1netid': PLAYERS[0][0],
                'partner2netid': PLAYERS[1][0],
                'partner1score': '1',
                'partner2score': '3',
                'teamnumber' : '1'
            },
            'team2': {
                'partner1netid': PLAYERS[2][0],
                'partner2netid': PLAYERS[3][0],
                'partner1score': '7',
                'partner2score': '3',
                'teamnumber' : '2'
             }
        },
        headers={
            'Content-Type': 'application/json'
        }
    )
print "}"
print response.text


    

'''
            if t1p1 > max(t1p2, t2p1, t2p1):
                t1pts += 1
            elif t1p2 > max(t1p1, t2p1, t2p1):
                t1pts += 1
            elif t2p1 > max(t1p1, t1p2, t2p2):
                t2pts += 1
            else:
                t2pts += 1
'''
