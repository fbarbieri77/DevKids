# Jogo da Forca

# Escolher a palavra
# Mostrar o tamanho da palavra e posicao de cada letra
# O jogodor chuta a letra ou a palavra
# Se a letra fizer parte da palavra -> imprimir a letra na 
#  posicao correta
# Checar se completou a palavra
# Se a letra não fizer parte da palavra -> desenha uma parte do 
#  corpo
# Checar se enforcou
# Se chutar a palavra, checar se vencou ou não

import os
os.system('cls')

# Elementos da forca
forca = \
"______________________ \n" + \
"     | "

cabeca = \
"   _  _ \n" + \
"   o  o \n" + \
"  (  L ) \n" + \
"     O "

corpo = \
"  __||__ \n" + \
" |  ||  \ \n" + \
" @  ||   @  "

pernas = \
"   _||_ \n" + \
"  /    \ \n" + \
"_/      \_ "

morto = \
"        \n" + \
"   X  X \n" + \
"  (  L ) \n" + \
"     X "

# inicializacao das variaveis
erros = [cabeca, corpo, pernas]
qtdErros = 0
forcaFinal = forca

def imprimirStatus(resp):
    os.system('cls')
    print(resp)
    print(forcaFinal)

palavra = input("Digite a palavra para jogar: ").upper()

resposta = ""
for item in palavra:
    resposta += "x"
imprimirStatus(resposta)

jogando = True
while jogando:
    chute = input("Chute uma letra ou a palavra: ").upper()
    if len(chute) == 1 and chute in palavra:
        # incluir a letra na resposta no lugar do X
        # imprimir a resposta
        respostaAtual = ""
        for i in range(len(palavra)):
            if palavra[i] == chute:
                respostaAtual += chute
            else:
                respostaAtual += resposta[i]
        resposta = respostaAtual
        imprimirStatus(resposta)
        if palavra == resposta:
            print("Vencedor !!")
            jogando = False
    elif len(chute) == 1:
        # atualizar a forca
        # imprimir a resposta
        if qtdErros < 3:
            forcaFinal += "\n" + erros[qtdErros]
            qtdErros += 1
        else:
            forcaFinal = forca + "\n" + morto + "\n" + corpo + "\n" + pernas
            jogando = False
        
        imprimirStatus(resposta)
    
    else:
        # checar se acertou a palavra
        if palavra == chute:
            print("Vencedor !!")
            jogando = False
        elif len(chute) != len(palavra):
            print("Chute 1 letra ou a palavra final!")
        else:
            forcaFinal = forca + "\n" + morto + "\n" + corpo + "\n" + pernas
            jogando = False
            imprimirStatus(resposta)
            
            
print("A palavra era: " + palavra)