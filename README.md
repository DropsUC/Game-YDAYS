# S&S — Slime & Squelette

> **Résoudre ensemble, ou ne pas avancer du tout.**

Un jeu de puzzle coopératif 2D développé sous Unity dans le cadre des **YDays 2026** à Ynov Campus.

---

## Concept

**S&S** met en scène deux joueurs incarnant un **Squelette** et un **Slime** aux capacités complémentaires. Chaque niveau est une énigme qui ne peut être résolue qu'en combinant intelligemment les deux personnages. Ni l'un ni l'autre ne peut progresser seul.

---

## Mécaniques

### Squelette
- Lance sa **tête comme projectile** rebondissant
- La tête peut **pousser des objets** et des caisses
- La tête peut **déclencher des boutons** à distance
- Système d'états : Tête attachée → En main → En vol → Repousse

### Slime
- Peut **s'accrocher aux murs** et grimper à la verticale
- Sert de **trampoline** pour propulser le Squelette
- Peut passer dans des **chemins étroits** inaccessibles au Squelette
- **Wall jump** : se projette hors des murs

---

## Structure du projet

```
Assets/
├── Animations/
│   ├── Slime/          # Idle, Move, Jump
│   └── Squelette/      # Idle, Run, LancerTete, PrendreTete
├── Scripts/
│   ├── PlayerMovement.cs       # Contrôles & lancer de tête (Squelette)
│   ├── SlimeMovement.cs        # Contrôles & grimpe murale (Slime)
│   ├── HeadProjectile.cs       # Comportement projectile
│   ├── GroundButton.cs         # Bouton au sol (déclenché par joueurs ou tête)
│   ├── TrampolineBounce.cs     # Mécanique trampoline
│   ├── PortalEndLevel.cs       # Portail de fin (requiert les 2 joueurs)
│   ├── VoidTrigger.cs          # Zone de mort / reset
│   ├── GameManager.cs          # Singleton, gestion de scène
│   ├── DialogueManager.cs      # Dialogues
│   ├── CameraRoomManager.cs    # Gestion caméra par salle
│   └── CameraSequenceManager.cs
├── Scenes/
│   ├── SampleScene.unity
│   └── Scene1.unity            # Niveau tutoriel
├── Imports/                    # Sprites & prefabs
└── Lucie/ Ugo/                 # Dossiers de travail par membre
```

---

## Lancer le projet

### Prérequis
- **Unity 6000.3.8f1** (Unity 6)
- Universal Render Pipeline (URP) 2D
- Input System package

### Installation
```bash
git clone https://github.com/DropsUC/Game-YDAYS.git
```
1. Ouvrir **Unity Hub**
2. Cliquer sur **Open** et sélectionner le dossier cloné
3. Attendre l'import des packages
4. Ouvrir `Assets/Scenes/Scene1.unity`
5. Appuyer sur **Play** 

### Contrôles (2 joueurs)
| Action | Squelette (J1) | Slime (J2) |
|--------|---------------|------------|
| Déplacement | `Q / D` | Flèches `←→` |
| Saut | `Z` | Flèche `↑` |
| Action | `Clic gauche` (lancer tête) | — |

> Les contrôles sont configurés via **Unity Input System** — voir `Assets/Controles.inputactions`

---

## Équipe

| Membre | Rôle |
|--------|------|
| **Ugo Coste** | Chef de groupe — Gameplay & personnages |
| **Adrien Yapoudijian** | Design des personnages & animations |
| **Lucie Barrez** | Level design & construction des niveaux |

---

## Roadmap

| Période | Milestone |
|---------|-----------|
| Oct 2025 | Brainstorm & répartition des rôles |
| Déc 2025 | Design Slime finalisé |
| Jan 2026 | Personnages terminés |
| Mar 2026 | Niveaux définis |
| Avr 2026 | Jeu jouable + debug |
| Mai 2026 | Livraison finale |

---

## Stack technique

- **Moteur** : Unity 6000.3.8f1
- **Rendu** : Universal Render Pipeline 2D (URP)
- **Langage** : C#
- **Input** : Unity Input System
- **UI** : TextMeshPro
- **Assets** : Cainos — Pixel Art Platformer Village Props

---

## Livrable

- ✅ 1 niveau tutoriel jouable
- ✅ 2 personnages avec animations complètes
- ✅ Mécaniques coopératives fonctionnelles
- ✅ Puzzles testés et solubles

---
