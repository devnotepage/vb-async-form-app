git pull -v --progress "origin"
printf "\n"

git push -v --progress "origin" master:master
printf "\n"

read -p "Complete!"
