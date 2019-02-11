# Countdown-Buddy
Countdown Buddy is a tool for the precise timing of astronomical events like transits or eclipses.

**Normal mode:**

![Screenshot of Countdown Buddy program](/screenshots/countdown-buddy.jpg?raw=true "Countdown Buddy")

**Settigns mode:**

![Screenshot of Countdown Buddy program showing settings mode](/screenshots/countdown-buddy-settings.jpg?raw=true "Countdown Buddy Settings Mode")

## Windows setup
To use the app on Windows simply download the countdownBuddy.exe file from the [releases folder.](/releases) The program can be run from anywhere on your computer.

## Mac OS / Linux setup
To use the app on Mac OS or Linux you need [Mono project.](https://www.mono-project.com/) 

Start by downloading the countdownBuddy.exe file from the [releases folder.](/releases)

Next install [Mono project for your operating system.](https://www.mono-project.com/download/stable/#download-mac)

After installing Mono project simply [run the program with Mono project from the command line.](https://www.mono-project.com/docs/about-mono/supported-platforms/macos/)

## Contents

[1. Basic operation](#1-basic-operation)

[2. Entering events time](#2-entering-events-time)

[3. Using voice plans](#3-using-voice-plans)

## User guide

### 1. Basic operation
Countdown Buddy is used to countdown the time to an event. This appication allows for the exact time to any event to be displayed and optionally read out at custom defined periods, this is especially useful for timing when an eclipse will occur.

Countdown Buddy has three main components: The big timer, the event details pane and the settings pane.

The big timer displays in hh:mm:ss.ff format with the - sign indicating that it is counting down to the event.

The event details pane displays how long until the event will occur, the duration of the event, when the halftime will occur and when the event will end. All of these values are automatically calculated based on what is entered in the settings pane. The settings button is used to hide the settings pane to prevent accidental changing of the settings.

The settings pane allows for all of the information needed for the countdown to be entered. Only two of the three fields needs to be filled out and the rest will automatiaclly be calculated. Voice plans can also be loaded in from this pane.

### 2. Entering events time
For Countdown Buddy to begin a countdown you first need to fill out when the event is happening! On startup the systems current time is used as the default event time.

To start fill out one of the three fields, the format is hh:mm:ss.ff and must be less than 24 hours. After filling out two of the three fields the last field will be calculated automatically.

After entering the event details it is a good idea to press the settings button again to hide the settings pane to prevent unintended changes.

### 3. Using voice plans
Voice plans allow you to add spoken sentences to your countdown, this is especially useful if you want to keep track of how much time you have left without staring at a screen.

Voice plans are controlled via specially formatted text files that tell Countdown Buddy what to say when.

The fastest way to get started with voice plans is to click on the "Generate example voice plan" button. This will automatically generate an example of what a voice plan should look like.

#### Voice plan basics:
Comments can be made with // at the beginning of a line

The format for voice events is t-00:00:00.00,r0,hello world
All fields are seperated with a comma

Time in countdown goes first
Leading with - sign indicates voiceEvent to play before event

Time formats:
t-10:00:00 = 10 hours before event
t-10:00 = 10 minutes before event
t-10 = 10 seconds before event
t-00.10 = 10 microseconds before event
t20 = 20 seconds after event
t30:00 = 30 minutes after event

An optional speech rate (within -10 to 10) can be specified
Speech rate controls how fast the message players
Leaving it blank will default it to zero

The last field is the message you want spoken
Anything placed after the last comma will be spoken

Voice plans only get executed once
To run a second time simply reload the voice plan






--------------------------------------
If you're experiencing any problems then you can open a new issue under the [issue tab.](../../issues)

**Clear Skies,**

**Jameson**
