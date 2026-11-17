# Dead Air — Loot & Inventory System

A Unity 3D technical test project for a Unity Developer position.

A prototype loot and inventory system for an action RPG set in the industrial, retro-futuristic world of **Dead Air**.

## Project Overview

The project demonstrates a modular loot and inventory system implemented using Unity and C#.

## Implemented Features

- **Item Data:** Items are configured using ScriptableObjects, including category, weight, stack size, pickup mode, and dismantling results.
- **Inventory System:** Weight-limited inventory with validation that prevents picking up items when the maximum capacity is exceeded.
- **Loot Pickup:** Supports both automatic and manual item pickup.
- **Loot Indicators:** Amber bracket indicators appear above nearby loot objects within the detection radius and face the player's camera.
- **Item Dismantling:** Items can be dismantled directly from the inventory into useful components.
- **Inventory UI:** Displays collected items, quantities, and current inventory weight.
- **Inventory Controls:** Inventory can be opened and closed using the keyboard.

## Technical Details

- **Engine:** Unity
- **Programming Language:** C#
- **Platform:** PC
- **Render Pipeline:** Built-in Render Pipeline
- **Architecture:** ScriptableObjects and modular gameplay components

## Controls

| Key | Action |
|---|---|
| E | Pick up nearby manually collectible items |
| I | Open / close inventory |

Automatic items are collected when the player enters their pickup radius.

## Inventory Rules

- Maximum inventory weight: 20 units.
- Items cannot be picked up if their weight would exceed the inventory limit.
- Junk items with configured dismantling results can be dismantled from the inventory.
- Dismantling is prevented if the resulting inventory weight would exceed the limit.

## Project Status

**Core loot and inventory mechanics implemented and tested.**

## Author

Dmitry Pyatnitsky

Unity Developer Candidate