

The GB and players connect using WebSockets to the Hub of their associated game (UneHub for UneGame).


Each player needs to store in memory:
1. Name (unique)
2. ConnectionId
3. Avatar


Each room needs to store:
1. GB id (easy access)
2. Players


Each game should be able to:
1. Pause
2. Resume
3. StartGame
4. FinishGame
5. KickPlayer
6. SendAvatars (w/ names)


Each game should have the ability to support:
1. A GB creating and joining a room.
2. A PL joining a room with a code.
3. A PL joining a room but not a game, can only queue to play in the next game, or round.


Une
* Game
* CircularOrder
1. JumpIns
2. Reverse

Draw()
DrawMultiple(int)
Play(card)
Play(cards)


CAH
* Game
* Rounds
* CircularOrder (Strict)









Games
1. Une
2. CAH
3. Poker
4. Blackjack
5. Tens