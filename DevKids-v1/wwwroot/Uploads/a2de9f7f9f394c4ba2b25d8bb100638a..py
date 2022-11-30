# inputs de teclado

import os
os.system('cls')

valorInput = input("Qual o seu nome? ")
idade = input("Qual a sua idade? ")
decadas = int(idade) / 10
# o resultado de um input() é uma str
print("Seu nome é: " + valorInput)
print("Sua idade é: " + idade)
print("Você tem " + str(decadas) + " decadas de idade")
print()