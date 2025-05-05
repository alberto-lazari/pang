# Pang Clone

**Try it on [itch.io](https://alberto-lazari.itch.io/pang) from your browser!**

A clone of a simple 2D arcade shooter, made in Unity.

Developed as a homework for the Mobile course of the Master in Computer Game Development.

It features almost every basic game development element
(player controller, animations, collisions, items, game state, UI).


## Aim of the Project

Learn the basics of Unity development by replicating the game [Pang](https://en.wikipedia.org/wiki/Buster_Bros.). \
As a simple 2D arcade game, it was a simple–yet complete–example,
requiring the implementation of various details, spanning almost the entire essential Unity toolset.

Being a mobile course, the project had to be developed with a mobile-first strategy in mind,
so touch input alternatives were required.

An emulated web version of the game is available
[here](https://www.miniplay.com/game/pang) for a comparison with the original game.


## Technical Details

While working on this project I had to learn many aspects of Unity development for the first time.
As all first-time projects,
many parts of this game were written before I learned more structured and elegant development patterns,
so don't expect to find anything well-polished or consistent.

Below I list the most relevant topics I worked on this project:
- Unity's prefab/hierarchy: very useful when mastered,
  can be used to achieve a great re-use of game elements
- Basic sprite animations
- Rigidbody collisions
- Trigger collisions: these are still not totally clear to me.
  They seem too much "exploitable", allowing a wide variety of use-cases. \
  The line between trigger and collision is sometimes slightly blurred.
  Also, I couldn't find an elegant pattern to manage layer filtering.
- Player controller: this was much harder than expected.
  The final result is a mess and the part I'm least proud of. \
  Handling climbing movement and Y-axis positioning in general was a real pain,
  and there are still strange bugs hanging around
  (mostly concerning ladders).
- UI
- Input, using the new Input System
- Global game state: I'm not sure how much I'm supposed to use singletons and global state in general.
  I found out about events later in the project and realized they are a much simpler and elegant solution.
  OOP feels very limited in Unity's game object model.


## Credits

- Sprites:
  [Player](https://www.spriters-resource.com/arcade/pangbusterbrospompingworld/sheet/32437),
  [Items/Misc](https://spritedatabase.net/game/3097)
- Font: [Departure Mono](https://departuremono.com)
- Up/Down buttons icon: [Up](https://www.flaticon.com/free-icon/up_12547950)
  by Disha Vaghasiya on Flaticon
- Bubble pop sound: Sound Effect by
  [freesound_community](https://pixabay.com/users/freesound_community-46691455/?utm_source=link-attribution&utm_medium=referral&utm_campaign=music&utm_content=91931)
  from [Pixabay](https://pixabay.com//?utm_source=link-attribution&utm_medium=referral&utm_campaign=music&utm_content=91931)
