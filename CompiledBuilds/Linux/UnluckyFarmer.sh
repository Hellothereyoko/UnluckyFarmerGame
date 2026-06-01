#!/bin/sh
printf '\033c\033]0;%s\a' UnluckyFarmer
base_path="$(dirname "$(realpath "$0")")"
"$base_path/UnluckyFarmer.x86_64" "$@"
