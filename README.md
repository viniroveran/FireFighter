## FireFighter Game for Game Engine I

### User Story 1
- [x] If the number of fires is equal to 0, we need to trigger a victory end screen to display results. If we hit any key, we load the next level (or the same level again if we only have one).
- [x] We define how many fires are lit per level (1 to 6)

### User Story 2
- [x] The game starts with a specific number of fire (1 to 6), after a delay (20 seconds or less) a new fire will start at a random window (that did not have a fire before.)
- [x] After we end the level, we give points (between 50 to 150 pts) based on the windows that did not caught on fire. ((14 – totalFires) * preventionScore)
- [x] We need to display the number of active fires in the UI.

### User Story 3
- [x] We need to measure the time from the beginning of the level and stop the clock when we finish.
- [x] We need to define an average time for each level (manually set, based on previous attempt).
- [x] We need to define a base score to grade the player (between 150 to 500 pts).
- [x] When we finish a level, we display a summary and a score based on how long it took to complete the level. ((avgTime / timeElapsed) * timeScore);
- [x] We need to display the results with details in the victory end screen.
- [x] We need to display the elapse time in the UI.

### User Story 4
- [x] We need to measure the damage amount based on how many fires are lit, and for how long (between 0.5 to 5% / second).  damage += (damageSpeed * activeFire) * Time.deltaTime;
- [x] When we complete a level, we need to give a score penalty (-10 pts/ 1%). **_Final score cannot be less than 0!_** (damage * penaltyScore);
- [x] If we reach 100% damage, the game ends with a Game Over screen.
- [x] When Game Over, we can restart the game from level 1 or exit game.
- [x] We need to display the damage in the UI.

### User Story 5
- [x] We need to specify an amount of water consumed per level (between 0.05 and 1%/second active). **_The level only goes down when we use it._**
- [x] If we ran out of water, the game ends with Game Over.
- [x] We need to specify different amount per level, to make the game more intense.
- [x] Every level must be beatable! – Test your late levels to make sure it is playable.
- [x] When we complete a level, we need to display a score based on the amount of water remaining (1000 pts/1%). (waterLeft * scoreWater);
- [x] We need to display the amount of water remaining in the UI.

### User Story 6
- [x] Slowly increase the intensity back when the value is below the original intensity (3) if we are not trying to extinguish it.
- [x] Invoke the increase only after a delay (1 second) when we previously tried to extinguish the fire.

### User Story 7
- [x] Add a high score in PlayerPrefs for each level.
- [x] Display the best score in the victory screen.

### User Story 8
- [x] Add a main menu with 2 buttons: Start Game, Exit game.
- [x] When we’re Game Over, we need to replace the button Exit game by Go to Main menu.
- [x] Add loading screen between scenes in the Build Settings.
- [x] The UI must always look good on different resolutions screen and aspect ratio.

### User Story 9
- [ ] Create between 5 to 10 levels with increasingly more difficulty in the game manager.
- [ ] Create an ending screen to say congratulations to winning player and then add a button to return to the main menu (or quit the game if you don’t have any.)

### User Story 10
- [x] Add an icon for the build.
- [x] Add a Config dialog image.
- [x] Remove unused inputs in the input manager.

# Credits
**All assets provided by Marc-André Larouche**

**Flame font: https://www.1001freefonts.com/flames.font, Font License: Free**

**Firefighter Icon: https://www.flaticon.com/free-icon/firefighter_206877**

**LaSalle College Montreal logo: https://www.lasallecollege.com/student-ressources/wifi**

**Siren ambiance sound: https://freesound.org/people/contramundum/sounds/322958/, License: Creative Commons 0**
