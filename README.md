
# 🏴‍☠️ La Carte aux Trésors — Simulateur de Déplacements (.NET 10)

Ce projet implémente un système robuste de suivi et de simulation de déplacements d'aventuriers au sein du département 
de la **Madre de Dios**, développé selon les exigences strictes de qualité, de lisibilité et de testabilité du gouvernement péruvien.

L'application est propulsée par **.NET 10** et s'appuie sur une architecture moderne en **Vertical Slice (Tranches Verticales)**.

---

##  Choix Architecturaux : Vertical Slice Architecture

Contrairement aux approches traditionnelles en couches (N-Tier, Onion ou Clean Architecture globale) qui séparent 
le code par nature technique, l'architecture en **Vertical Slice** regroupe le code par **fonctionnalité (Feature)**. 

## Utilisation de l'Application

Pour lancer la simulation, assurez-vous d'avoir un fichier inputFile.txt valide à la racine du projet ou modifiez le chemin
dans Program.cs, puis lancez:
dotnet run --project src/TreasureMap/TreasureMap.csproj

### Format du fichier d'entrée (input.txt)
```text
C - 3 - 4
M - 1 - 0
M - 2 - 1
T - 0 - 3 - 2
T - 1 - 3 - 3
A - Lara - 1 - 1 - S - AADADAGGA

---
### Format du fichier de sortie généré (ouput.txt)
```text
C - 3 - 4
M - 1 - 0
M - 2 - 1
T - 1 - 3 - 2
A - Lara - 0 - 3 - S - 3

---






