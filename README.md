<p align="center">
  <img src="https://static.wixstatic.com/media/cded79_e86e5c951f554d53b4167c3adb36856d~mv2.png/v1/fill/w_178,h_315,al_c,q_85,usm_0.66_1.00_0.01,enc_auto/logoshadow.png">
</p>

<p align="center">
  <b>~ Nicknames ~</b><br>
  <b>Created for the Oceanic Bannelord Community</b>
<br>
https://discord.gg/SKFTPet994
</p>

# Notice

**Are you a regular user?**

This page is designed for **server owners** looking to implement this on their server.<br>
As a **regular player**, you don't need to worry about this page.

# What is this?

This module allows player's to change their nickname in native multiplayer, and enables the client-side module to work on servers with this module activated.

This is a part of a series of Bannerlord multiplayer improvements.
- [OCEAdmin](https://github.com/Oceanic-Bannerlord-Official/OCEAdmin) - server moderation, bans, mutes and admins. 
- Admin panel[^1] - in-game admin panel for native server moderation, compatibility with ChatCommands and OCEAdmin.

[^1]: Work in progress - broken by latest updates.

# Supported modules

All modules are supported except Sword & Musket, which already has it's own implementation.

## Server Installation

1. Download the latest release.
2. Drag and drop into your Bannerlord Dedicated Server's module folder.
3. Make sure to include *Nicknames into your command options for startup.

# How does it work?

**Authoriative server**

Servers/players using the module will communicating with the master server I have hooked up.
It has been thoroughly tested in terms of scalability, having no performance impact up to 300+ players.

**Why not just have the client store nicknames?**

I've gone with this approach because the API this is attached to will include more than just nicknames in the future.
It requires OAuth authentication processes for Steam, Microsoft and Epic games accounts.
