# funcoes
import os
os.system('cls')

# exemplo 1
print("Exemplo 1 \n")
palavra = "casa"
letra = "d"
# se a letra c faz parte de casa
if letra in palavra:
    print("Letra " + letra + " faz parte de " + palavra)
else:
    print("Letra não faz parte")
    
    
# exemplo 2: com funcao
def checarLetra(letra):
    if letra in palavra:
        print("Letra " + letra + " faz parte de " + palavra)
    else:
        print("Letra " + letra + " NÂO faz parte de " + palavra)
        
print("\n\nExemplo 2\n")
palavra = "Sorocaba"

letras = ['d', 'a', 'b', 'm']


for var in letras:
    checarLetra(var)
    

