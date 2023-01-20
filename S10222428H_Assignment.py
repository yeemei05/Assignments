#Ho Yee Mei (S10222428H) -  9 August

def Header():
    print('Welcome, mayor of Simp City!\n----------------------------')

removeLst = []

def displayMainmenu():

    global turn
    #printing main menu
    print('1. Start new game\n2. Load saved game\n3. Show high scores\n')
    print('0. Exit')
    global choice
    choice = input('Your choice? ')
    choiceLst = ['1','2','3','0']
    #if choice is valid
    if choice in choiceLst:
        if choice == '1':
            turn = 1
            #to clear board
            for index in removeLst:
                row = int(index[0])
                col = int(index[1])
                location[row][col]='   '
            start_newgame()
        elif choice == '2':
            load_game()                            
            start_newgame()
        elif choice == '3':
            printscoreboard()
            displayMainmenu()
        elif choice == '0':
            print('Thank you and Goodbye!')
            exit()
    #if choice is invalid
    else:
        while choice not in choiceLst:
            print('Invalid option. Please try again.\n')
            displayMainmenu()
            break

#-----------------------------------------------------------------------show game-----------------------------------------------------------------------
turn = 1
location = [['   ','   ','   ','   ','   ','   '],\
            ['   ','   ','   ','   ','   ','   '],\
            ['   ','   ','   ','   ','   ','   '],\
            ['   ','   ','   ','   ','   ','   '],\
            ['   ','   ','   ','   ','   ','   '],\
            ['   ','   ','   ','   ','   ','   ']]
def print_board():
    print()
    global board, turn
    if turn <= 16:
        print("Turn", turn) #printing the turns   
    else:
        print('Final layout of Simp City:') #printing the final layout 

    #printing the board   
    print('     A     B     C     D')
    print('  +-----+-----+-----+-----+')
    for r in range(1,5):
        print(' {}'.format(r),end='')
        for c in range(1,5):
            print('| {:3} '.format(location[r][c]), end = '')
        print('|')

        print('  +-----+-----+-----+-----+')

            
#-----------------------------------------------------------------------load game-----------------------------------------------------------------------
def load_game():
    global turn,countlist, removeLst, listbuild
    file = open('simpCity.txt','r')

    turn = int(file.readline()) #getting the number of turns
    #loading the board (location) out
    row = 0
    listbuild = []
    for line in file:
        line = line.strip('\n')
        data_list = line.split(',')

        for column in range(0,6):
            location[row][column] = data_list[column]
            listbuild.append((data_list[column]))
            
        row += 1
        if row == 6:
            break
    #getting the countlist from saved
    tcountlist = file.readline()
    
    #getting the removelist from saved game    
    tremoveLst = file.readline()
    tremoveLst = list(tremoveLst.split(","))
    tremoveLst = tremoveLst[:-1]
    removeLst = []
    for r in tremoveLst:
        removeLst.append(r)

#----------------------------------------------------------------------save game----------------------------------------------------------------------
def save_game():
    file = open("simpCity.txt", 'w')
    #saving the turn 
    file.write(str(turn) + '\n')
    #saving the board data
    for row in range(0,6):
        data =''
        for column in range(0,6):
            data = data + location[row][column]+','

        file.write(data + '\n')
    #saving the countlist
    file.write(str(countlist)+'\n')

    #saving removal list
    removal = ''
    for x in range(len(removeLst)):
        removal = removal + removeLst[x] + ','
        
    file.write(removal)
        
    file.close()            

#-----------------------------------------------------------------------game menu------------------------------------------------------------------------
def gamemenu():
    #printing the options for user when turn <= 16
    if turn <= 16:
        global building1
        global building2
        global buildings, index1, index2
        buildings = ['HSE','SHP','FAC','HWY','BCH','PRK','MON']
        #checking if the buildings would still be able the built
        #if there is still remaining buildings for the user to built
        if counthse == 0:
            buildings.remove('HSE') # removing if no more hse can be built
        if countshp == 0:
            buildings.remove('SHP') # removing if no more shp can be built
        if countfac == 0:
            buildings.remove('FAC') # removing if no more fac can be built
        if counthwy == 0:
            buildings.remove('HWY') # removing if no more hwy can be built
        if countbch == 0:
            buildings.remove('BCH') # removing if no more bch can be built
        if countprk == 0:
            buildings.remove('PRK') # removing if no more prk can be built
        if countmon == 0:
            buildings.remove('MON') # removing if no more mon can be built

            
        buildinglist = []
        #creating 8 of each building
        for count in range(8):
            for b in range (len(buildings)):
                buildinglist.append(buildings[b])
        #calling random building for each choices
        import random
        index1 = random.randint(0, len(buildinglist) -1)
        building1 = buildinglist[index1]
        index2 = random.randint(0, len(buildinglist) -1)
        building2 = buildinglist[index2]
        #printing the choices for user the choose
        print('1. Build a {}'.format(building1))
        print('2. Build a {}'.format(building2))
        print('3. See remaining buildings')
        print('4.See current score\n\n5. Save game\n0. Exit to main menu')
        
    else: #for the end of game, after printing final layout
        countscore() #display score
        highscore() #show highscore board
        print()
        displayMainmenu() #display main menu after end of game
        
#----------------------------------------------------------------building score-------------------------------------------------------------------------
def countscore():
    global buildingscore, total
    # dictionary for score
    buildingscore = {'HSE':[], 'FAC': [], 'SHP':[], 'HWY':[], 'BCH':[],'PRK':[],'MON':[]}

    counterpark = 0
    parksurround = []
    parkgridlist=[]
    parkgrid = []
    counter = 0
    countFac = []
    hselist= []
    cornermon = 0
    #for mon corner count, to count the number of MON in the corner
    if location[1][1] == 'MON':
        cornermon += 1
    if location[1][4] == 'MON':
        cornermon += 1
    if location[4][1] == 'MON':
        cornermon += 1
    if location[4][4] == 'MON':
        cornermon += 1

    #calling each location
    for lr in range(1,5):
        for lc in range(1,5):
            
            #beach score
            if location[lr][lc] == 'BCH':
                if lc == 1 or lc == 4: #built in column A or D
                    buildingscore['BCH'].append(3)
                else:
                    buildingscore['BCH'].append(1) #otherwise 1 point

            #park number of buildings
            elif location[lr][lc] == 'PRK':
                parksurroundlist = []
                parksurround.append(location[lr+1][lc])
                parksurround.append(location[lr][lc+1])
                parksurround.append(location[lr-1][lc])
                parksurround.append(location[lr][lc-1])
                if 'PRK' not in parksurround:
                    buildingscore['PRK'].append(1)
                else:
                    centerindex = str(lr) + str(lc) #getting the current index 
                    #print(centerindex)
                    #checking for center index, whether it is in the big parkgrid,
                    #len(parkgridlist)!=0 prevents from appending empty list   
        
                    if centerindex not in parkgridlist and len(parkgridlist)!=0:
                        if len(parkgrid) >= 1:
                            for lst in parkgrid:
                                if centerindex in lst:
                                    parkgrid.append(parkgridlist)
                                    parkgridlist = lst
                        else:
                            parkgrid.append(parkgridlist)
                            parkgridlist = [] #calling a new list to allow the next set of park that are not linked
            
                    if centerindex not in parkgridlist: # current index (CENTER)
                        parkgridlist.append(centerindex) 
                    if location[lr][lc+1] == 'PRK': # checking the RIGHT of the park
                        row = str(lr)
                        column = str(lc+1)
                        index = row + column
                        if index not in parkgridlist:
                            parkgridlist.append(index)
                    if location[lr][lc-1] == 'PRK': #checking the LEFT of the park
                        row = str(lr)
                        column = str(lc-1)
                        index = row + column
                        if index not in parkgridlist:
                            parkgridlist.append(index)
                    if location[lr-1][lc] == 'PRK': #TOP
                        row = str(lr-1)
                        column = str(lc)
                        index = row + column
                        if index not in parkgridlist:
                            parkgridlist.append(index)
                    if location[lr+1][lc] == 'PRK': # BOTTOM
                        row = str(lr+1)
                        column = str(lc)
                        index = row + column
                        if index not in parkgridlist:
                            parkgridlist.append(index)
                    #print(parkgridlist)
                    
            #shopping score
            elif location[lr][lc] == 'SHP':
                adj_list = []
                #first building
                adj_list.append(location[lr - 1][lc])
                #checking if it's in the list, else append(new building type)
                if location[lr+1][lc] not in adj_list:
                    adj_list.append(location[lr+1][lc])
                if location[lr][lc - 1] not in adj_list:
                    adj_list.append(location[lr][lc - 1])
                if location[lr][lc + 1] not in adj_list:
                    adj_list.append(location[lr][lc + 1])
                # '   ' is not a building, to prevent it to be counted in the len() remove it
                if '   ' in adj_list:
                    adj_list.remove('   ')
                #len(list) would be the score as it is the number of different buildings built around it
                buildingscore['SHP'].append(len(adj_list))
                
            #highway score
            elif location[lr][lc] == 'HWY':
                #ensuring no hwy before the current hwy
                if location[lr][lc - 1] != 'HWY':
                    score = 0
                    #if the next one is hwy add 1 pt
                    while  location[lr][lc] == 'HWY':
                        score += 1
                        lc += 1
                    #to have the number of times
                    for squares in range(score):
                        buildingscore['HWY'].append(score)
                        
            #house list
            #to have the value of the lr and lc 
            elif location[lr][lc] == 'HSE':
                hselist.append(str(lr)+str(lc))

                
            #fac list
            #to have a list for the number of fac
            elif location[lr][lc] == 'FAC':
                countFac.append('FAC')

            #mon score
            elif location[lr][lc] == 'MON':
                #corner matrix 
                corner = ['11','14','41','44']
                #if there's less than 3 mon in the corner
                if cornermon < 3:
                    locate = str(lr)+str(lc)
                    if locate in corner:
                        mscore = 2
                    else:
                        mscore = 1
                elif cornermon >= 3:
                    mscore = 4
                buildingscore['MON'].append(mscore)

    parkgrid.append(parkgridlist)

    # Check for any duplicated list in the parkgrid
    filteredPG = []
    for i in parkgrid:
        if i not in filteredPG:
            filteredPG.append(i)
    
    #print(filteredPG)
    #getting the score for the size of the park
    for lst in filteredPG:
        size = len(lst)
        #print(size)
        if size == 1:
            score = 1
        elif size == 2:
            score = 3
        elif size == 3:
            score = 8
        elif size ==4:
            score = 16
        elif size == 5:
            score = 22
        elif size == 6:
            score = 23
        elif size == 7:
            score = 24
        elif size == 8:
            score = 25
        elif size == 0:
            score = 0
        buildingscore['PRK'].append(score)
    #print(buildingscore['PRK'])

           
    #getting hse score
    for hse in hselist:
        #converting back into integer
        lr = int(hse[0])
        lc = int(hse[1])
        hsescore = 0
        score = 0
        #to find the surrounding buildings of the house
        surroundingbuildings = []
        surroundingbuildings.append(location[lr+1][lc])
        surroundingbuildings.append(location[lr][lc+1])
        surroundingbuildings.append(location[lr-1][lc])
        surroundingbuildings.append(location[lr][lc-1])
        #if there is fac is surrounding score = 1
        if 'FAC' in surroundingbuildings:
            hsescore = 1
        #if there is no fac
        else:
            for build in range(len(surroundingbuildings)):
                if surroundingbuildings[build] == 'HSE':
                    score= 1
                if surroundingbuildings[build] == 'SHP':
                    score=1
                if surroundingbuildings[build] == 'BCH':
                    score=2
                if surroundingbuildings[build] == 'HWY':
                    score=0
                if surroundingbuildings[build] == '   ':
                    score=0
                hsescore += score
        buildingscore['HSE'].append(hsescore)
    #print(parkgrid)
    

                
    #fac score            
    FAC = []
    #number of factory
    score = len(countFac)
    #if there is 5 or more
    if score > 4:
        #the first 4 fac
        for fac in range(4):
            FAC.append(4)
        #the next few
        for fac in range(score-4):
            FAC.append(1)
    #if there is less than 5
    elif score <= 4:
        for z in range(score):
            FAC.append(score)

    for score in FAC:
        buildingscore['FAC'].append(score)

    
    stringscorelist =[]
    total = 0
    #to print out score
    for key in buildingscore:
        #to find out score from dictionary
        score_list = buildingscore[key]
        #printing out name for buildings
        print(key, end = ' : ')
        #making it in str
        stringscorelist = [str(score) for score in score_list]
        #add the '+' sign
        includesign = ' + '.join(stringscorelist)
        #sum of the building
        sumscore =sum(score_list)
        #to get the total of all buildings
        total+= sumscore
        #to prevent '=0' to get printed out when its only 0
        if sum(score_list) == 0:
            print('{}'.format(sumscore))
        #printing out string and sum
        else:
            print('{} '.format(includesign),end = '= ')
            print('{}'.format(sumscore))
    print('Total score: {}'.format(total)) #printing out the total
                
#----------------------------------------------------------------exit game----------------------------------------------------------------------------------
def exitgame():
    print()

    displayMainmenu()
#----------------------------------------------------------------to find remaining buildings----------------------------------------------------------------
def counting():
    global countlist, counthse, counthwy, countfac, countshp, countbch, countprk, countmon
    countlist =[]
    counthse = 8
    counthwy = 8
    countfac = 8
    countshp = 8
    countbch = 8
    countprk = 8
    countmon = 8
    #to call for the location and check whether it is that building
    for h in range(len(location)):
        for i in location[h]:
            if i == 'HSE':
                counthse -=1
            elif i == 'FAC':
                countfac -=1
            elif i == 'SHP':
                countshp -=1
            elif i == 'HWY':
                counthwy -=1
            elif i == 'BCH':
                countbch -=1
            elif i == 'PRK':
                countprk -= 1
            elif i == 'MON':
                countmon -= 1
    #append it into the list
    countlist.append(counthse)
    countlist.append(countfac)
    countlist.append(countshp)
    countlist.append(counthwy)
    countlist.append(countbch)
    countlist.append(countprk)
    countlist.append(countmon)
    return countlist

        
#----------------------------------------------------------------start new game------------------------------------------------------------------------
def start_newgame():
    global turn, row, col, removeLst
    buildinglist =[]
    #different type of buildings
    buildings = ['HSE', 'FAC', 'SHP', 'HWY', 'BCH','PRK','MON']
    buildlst = ['a1','b1','c1','d1','a2','b2','c2','d2','a3','b3','c3','d3','a4','b4','c4','d4',\
                'A1','B1','C1','D1','A2','B2','C2','D2','A3','B3','C3','D3','A4','B4','C4','D4']

    quit = False
    #to have 8 each for each buildings
    for count in range(8):
        for b in range (len(buildings)):
            buildinglist.append(buildings[b])
    #to count the building, to check whether it is =0 or to print from saved board
    counting()
    

       
    while not quit:
        print_board()
        gamemenu()
        counting()
        option = input("Your choice? ")
        if option == '1':
            build = input('Build where? ')
            #to check validation of build building
            if build not in buildlst:
                print('Invalid option. Please try again.')
            else:
                row = int(build[1])
                col = ord(build[0].upper()) - ord('A') + 1
                #no validation 
                if turn == 1:
                    location[row][col] = building1
                    removeLst.append(str(row)+str(col)) #to find the location if the user want to start new game, can be removed
                    buildinglist.remove(building1)
                    turn += 1
                elif turn >= 2:
                    #to make sure it doesnt get replaced and ensure it is empty
                    if location[row][col] != '   ':
                        print('You must build on a blank space.')
                    else:
                        #to make sure that beside the choice theres and existing building
                        if location[row][col-1] == '   ' and location[row][col+1] == '   ' \
                           and location[row-1][col]== '   ' and location[row+1][col] == '   ':
                            print('You must build next to an existing building.')
                        else:
                            #building the building 
                            location[row][col] = building1
                            #converting into string before adding to list
                            removeLst.append(str(row)+str(col))
                            #to find the location if the user want to start new game, can be removed
                            buildinglist.remove(building1)
                            #only when building is successful, turn += 1
                            turn +=1


        elif option == '2':
            build = input('Build where? ')
            #to check validation of build building
            if build not in buildlst:
                print('Invalid option. Please try again.')
            else:
                row = int(build[1])
                col = ord(build[0].upper()) - ord('A') + 1
                #no validation
                if turn == 1:
                    location[row][col] = building2
                    removeLst.append(str(row)+str(col)) #to find the location if the user want to start new game, can be removed
                    buildinglist.remove(building2)
                    turn += 1

                elif turn >= 2:
                    #to make sure it doesnt get replaced and ensure it is empty
                    if location[row][col] != '   ':
                        print('You must build on a blank space.')
                    else:
                        #to make sure that beside the choice theres and existing building
                        if location[row][col-1] == '   ' and location[row][col+1] == '   ' \
                           and location[row-1][col]== '   ' and location[row+1][col] == '   ':
                            print('You must build next to an existing building.')
                        else:
                            #building the building
                            location[row][col] = building2
                            #converting into string before adding to list
                            removeLst.append(str(row)+str(col))
                            #to find the location if the user want to start new game, can be removed
                            buildinglist.remove(building2)
                            #only when building is successful, turn += 1
                            turn +=1
                
            

        elif option == '3': #finding the remaining buildings
            #count the buildings 
            counting()
            #printing the board
            print('Building            Remaining')
            print('--------            ---------')
            for p in range(len(countlist)):
                print('{}                  {}'.format(buildings[p],countlist[p]))

        elif option == '4': #counting score
            countscore()

        elif option == '0': #exiting game, going to main menu
            exitgame()

        elif option == '5': #saving game
            save_game()
            print()
            print('Game saved!')
        #validation for option
        else:
            print('Invalid option. Please try again.')

#---------------------------------------------------------------- high scorehighscorelist --------------------------------------
def highscore():#printing highscore board for end of game 
    #END OF GAME
    global highscorelist
    highscorelist = []

    #load highscore---------------------------------------------------------------------
    file = open("game.txt", "r")
    for line in file:
        line = line.strip("\n")
        line = line.split(',')
        highscorelist.append([line[0],int(line[1])])
    
    #if there's space, user would be able to enter the highscore board
    if len(highscorelist) <= 10:
        name = str(input("Please enter your name (max 20 chars): 0"))
        highscorelist.append([name,total])
        sort_highscores()   
    else:
        #checking whether the user would be able to enter the high score board
        for i in range(len(highscorelist)):
            if total > int(highscorelist[i][1]):
                name = str(input("Please enter your name (max 20 chars): "))
                name_list = [name,total]
                highscorelist.append([name,total])
                break
        sort_highscores()
        
    #save highscore---------------------------------------------------------------------
    #after user enter, it would save into the data
    file = open("game.txt", "w")
    for y in range(len(highscorelist)):
        data1 = str(highscorelist[y][0]) + ',' + str(highscorelist[y][1])
        file.write(data1 + '\n')

    file.close()
    #the position
    counter = 1
    #if the user is within the top ten, it will print congrats...
    if (highscorelist.index([name,total])+1) <= 10:
        print('Congratulations! You made the high score board at position {}!'.format((highscorelist.index([name,total]))+1))
    #print high score board
    print('--------- HIGH SCORES ---------')
    print('Pos Player                Score\n--- ------                -----')
    for a in range(len(highscorelist)):
        print('{:>2}. {:<25}{}'.format(counter, highscorelist[a][0], highscorelist[a][1]))
        counter +=1
        if counter >10:
            break #printing the top 10 position only
    print('------------------------------')

#print score board for option 3 in main menu    
def printscoreboard():
    #for main menu
    #read data from file to print out high score board
    highscorelist = []
    file = open("game.txt", "r")
    for line in file:
        line = line.strip("\n")
        line = line.split(',')
        highscorelist.append([line[0],int(line[1])])

    #the position
    counter = 1
    #print high score board
    print('--------- HIGH SCORES ---------')
    print('Pos Player                Score\n--- ------                -----')
    for a in range(len(highscorelist)):
        print('{:>2}. {:<25}{}'.format(counter, highscorelist[a][0], highscorelist[a][1]))
        counter +=1
        if counter >10:
            break #printing the top 10 position only
    print('-------------------------------')



#sorting the double list according to the user score
#using bubble sort
#If the first value is higher than second value, the first value takes the second value position, while second value takes the first value position.
def sort_highscores():
    #getting the length of highscore list
    length = len(highscorelist)
    for count in range(length):
        for i in range(length - count - 1):
            if (highscorelist[i][1] < highscorelist[i+1][1]): #first item in the list < second item in the list
                temporary_list = highscorelist[i] #to keep the first value
                highscorelist[i] = highscorelist[i + 1] #to move to the up the second one one (switch postition)
                highscorelist[i + 1] = temporary_list #to bring back the initial first value and then once agn compare
 
#-------------------------------------------------------------------- run game--------------------------------------------------------------------            
Header()
displayMainmenu()

    

        



