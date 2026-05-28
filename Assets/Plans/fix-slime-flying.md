# Problème : Le Slime "vole" et se met sur le côté
Le Slime détecte son propre corps comme s'il s'agissait d'un mur. Comme il pense toucher un mur, il désactive sa gravité (pour grimper) et pivote à 90 degrés. Comme le "mur" (lui-même) le suit partout, il reste bloqué dans cet état et "vole" sans redescendre.

# Solutions proposées

## Option 1 : Modifier le code pour ignorer son propre corps (Recommandé)
Nous allons modifier le script `SlimeMovement.cs` pour que les rayons de détection (Raycasts) ignorent le collider du Slime lui-même. C'est la solution la plus propre car elle permet de garder le Slime sur le layer "Ground" (pour que le squelette puisse rebondir dessus) tout en évitant qu'il ne se détecte lui-même.

**Pros :**
- Très robuste.
- Permet au squelette de continuer à rebondir sur le Slime.
- Règle définitivement le problème de rotation et de vol.

**Cons :**
- Nécessite une petite modification du code C#.

## Option 2 : Changer les Layers
Remettre le Slime sur le layer "Player" et modifier le masque de détection du squelette pour qu'il détecte à la fois "Ground" et "Player".

**Pros :**
- Pas de changement de code complexe.

**Cons :**
- Moins performant si vous ajoutez beaucoup de types d'objets.
- Plus complexe à gérer si vous avez d'autres ennemis ou objets interactifs.

# Recommandation
Je recommande l'**Option 1**. C'est la plus professionnelle et elle évite les comportements imprévus avec la physique.

# Étapes d'implémentation (Option 1)
1. **Modifier `SlimeMovement.cs`** :
    - Ajouter une variable pour stocker le Collider du Slime.
    - Utiliser `Physics2D.queriesStartInColliders = false` ou filtrer les résultats des Raycasts pour ignorer l'objet actuel.
    - Intégrer la variable `baseGravityScale` pour permettre de régler la gravité depuis l'Inspector sans qu'elle ne soit réinitialisée à 3.
2. **Vérification** :
    - Tester le déplacement horizontal pour vérifier que le Slime ne pivote plus sur lui-même.
    - Vérifier que le Slime peut toujours grimper aux VRAIS murs.

# Vérification & Test
- Lancer le mode Play.
- Se déplacer à droite et à gauche sur le sol plat. Le Slime doit rester horizontal.
- Sauter contre un mur. Le Slime doit pivoter et s'y accrocher.
- Faire sauter le squelette sur le Slime. Le rebond doit toujours fonctionner.
