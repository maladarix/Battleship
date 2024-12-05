# Battleship  
Par Matisse, Antoine et Anthony  

## Ajout de fonctionnalités  
- Ajout de difficultés (Easy, Normal, Hard)  
- Placement automatique des bateaux disponible  

## Mode Easy - *(96 coups pour terminer 50 % des parties)*  

Attaque aléatoirement partout sur le tableau.  

![image](https://github.com/user-attachments/assets/821fa892-5535-46a7-8529-9126d1ca1fdd)

## Mode Normal - *(65 coups pour terminer 50 % des parties)*  

Attaque aléatoirement jusqu'à ce que l'ordinateur touche un bateau. Quand un bateau est trouvé, il attaque les 4 côtés jusqu'à ce qu'il touche une 2e fois, ce qui lui donne la direction du bateau.  

Une fois la direction trouvée, il attaque dans cette direction jusqu'à ce que le bateau coule.  

![image](https://github.com/user-attachments/assets/3752f7d5-5161-4345-abd3-6ea51f88f48c)

## Mode Hard - *(42 coups pour terminer 50 % des parties)*  

Calcule automatiquement la probabilité de chaque case à contenir un bateau. Une fois fait, il prend la case avec la plus grande probabilité et l'attaque.  

![image](https://github.com/user-attachments/assets/ec26e81b-b91c-44cd-8a4e-a61f4c947ca3)

## Sources  
- Algorithmes d'attaque : [datagenetics.com](http://datagenetics.com/blog/december32011/index.html)
