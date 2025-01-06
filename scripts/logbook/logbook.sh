#!/bin/bash

read -p "Enter your Discord username: " discord_username

while [ -z "$discord_username" ]; do
    echo "Discord username cannot be blank. Please try again."
    echo
    read -p "Enter your Discord username: " discord_username
done

echo
echo "Confirm your launcher:"
echo "1) Official Launcher"
echo "2) CurseForge Launcher"
echo "3) Prism Launcher"
echo "4) Modrinth Launcher"
echo "5) Technic Launcher"
read -p "Enter your choice: " launcher_type

case $launcher_type in
    1)
        echo
        echo "Confirm your log type:"
        echo "1) Latest Log"
        echo "2) Launcher Log"
        read -p "Enter your choice: " log_type

        if [ "$log_type" == "1" ]; then
            log_name="latest"
        elif [ "$log_type" == "2" ]; then
            log_name="launcher"
        else
            echo
            echo "Invalid log type. Please try again."
            exec "$0"
        fi

        if [ "$log_name" == "latest" ]; then
            selected_directory="$HOME/Library/Application Support/minecraft/logs/latest.log"
            log_name="Latest Log"
        else
            selected_directory="$HOME/Library/Application Support/minecraft/launcher_log.txt"
            log_name="Launcher Log"
        fi
        ;;
    2)
        index=1
        prism_dirs=("$HOME/Documents/curseforge/minecraft/Instances"/*/)
        if [ ${#prism_dirs[@]} -eq 0 ]; then
            echo
            echo "No CurseForge installations found."
            exit 1
        fi

        echo
        echo "Your CurseForge installations:"
        for dir in "${prism_dirs[@]}"; do
            echo "$index) $(basename "$dir")"
            ((index++))
        done

        read -p "Enter the index of the installation you are using: " selected_index
        selected_dir=${prism_dirs[$selected_index-1]}

        selected_directory=$selected_dir"logs/latest.log"
        log_name="Prism Latest Log ($(basename "$selected_dir"))"
        ;;
    3)
        index=1
        prism_dirs=("$HOME/Library/Application Support/PrismLauncher/instances"/*/)
        if [ ${#prism_dirs[@]} -eq 0 ]; then
            echo
            echo "No Prism installations found."
            exit 1
        fi

        echo
        echo "Your Prism installations:"
        for dir in "${prism_dirs[@]}"; do
            echo "$index) $(basename "$dir")"
            ((index++))
        done

        read -p "Enter the index of the installation you are using: " selected_index
        selected_dir=${prism_dirs[$selected_index-1]}

        selected_directory="$selected_dir.minecraft/logs/latest.log"
        log_name="Prism Latest Log ($(basename "$selected_dir"))"
        ;;
    4)
        index=1
        modrinth_dirs=("$HOME/Library/Application Support/com.modrinth.theseus/profiles"/*/)

        if [ ${#modrinth_dirs[@]} -eq 0 ]; then
            echo
            echo "No Modrinth installations found."
            exit 1
        fi

        echo
        echo "Your Modrinth installations:"
        for dir in "${modrinth_dirs[@]}"; do
            echo "$index) $(basename "$dir")"
            ((index++))
        done

        read -p "Enter the index of the installation you are using: " selected_index
        selected_dir=${modrinth_dirs[$selected_index-1]}

        selected_directory=$selected_dir"logs/latest.log"
        log_name="Modrinth Latest Log ($(basename "$selected_dir"))"
        ;;
    5)
        index=1
        technic_dirs=("$HOME/Library/Application Support/technic/modpacks"/*/)
        if [ ${#technic_dirs[@]} -eq 0 ]; then
            echo
            echo "No Technic installations found."
            exit 1
        fi

        echo
        echo "Your Technic installations:"
        for dir in "${technic_dirs[@]}"; do
            echo "$index) $(basename "$dir")"
            ((index++))
        done

        read -p "Enter the index of the installation you are using: " selected_index
        selected_dir=${technic_dirs[$selected_index-1]}

        selected_directory=$selected_dir"logs/latest.log"
        log_name="Technic Latest Log \($(basename "$selected_dir")\)"
        ;;
    *)
        echo
        echo "Invalid launcher type. Please try again."
        exec "$0"
        ;;
esac

if [ ! -f "$selected_directory" ]; then
    echo
    echo "Log not found: $selected_directory"
    exit 1
fi

echo
echo "Your paste link:"
curl -H "title: $discord_username's $log_name" -X POST --upload-file "$selected_directory" https://api.pastebook.dev/upload
echo
echo "Log uploaded successfully."
