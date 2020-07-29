#!/usr/bin/env bash
### ==============================================================================
### OmegaGraf Installation Script
### ==============================================================================

readonly PROGVERS="0.0.1"
readonly PROGAUTH="me@jacobcolvin.com"
# uncomment next line to have time prefix for every output line
#prefix_fmt='+%H:%M:%S | '
readonly prefix_fmt=""

# runasroot = 0 :: don't check anything
# runasroot = 1 :: script MUST run as root
# runasroot = -1 :: script MAY NOT run as root
runasroot=-1

download() {
  announce "Setting up OmegaGraf"

  progress "Downloading OmegaGraf"
  curl -s "https://api.github.com/repos/OmegaGraf/OmegaGraf/releases/latest" |
    grep "OmegaGraf.*tar.gz" |
    cut -d : -f 2,3 |
    tr -d \" |
    wget -qi -
  success "Downloaded OmegaGraf"

  progress "Extracting binary"
  tar -xzf OmegaGraf*.tar.gz &&
    success "Extracted binary" ||
    die "Could not extract the OmegaGraf binary"

  progress "Setting permissions"
  chmod +x OmegaGraf &&
    success "Allowed execute on OmegaGraf binary" ||
    die "Could not set execute on the OmegaGraf binary"
}

prechecks() {
  announce "Preparing for installation"

  progress "Ensuring system is ready for installation"

  verify_programs awk curl wget cut tr tar gzip date echo find grep head printf sed stat tail uname wc docker

  is_empty $PACKAGE_MANAGER && die "Could not find a supported package manager"

  success "System is ready"
}

dependencies() {
  announce "Installing dependencies via $PACKAGE_MANAGER"

  sudo "$PACKAGE_MANAGER" update &&
    sudo "$PACKAGE_MANAGER" install apt-transport-https ca-certificates gnupg2 software-properties-common &&
    success "Dependencies have been installed"
}

cleanup() {
  announce "Cleaning up old OmegaGraf installations"

  for i in OmegaGraf*.tar.gz; do
    is_file "$i" &&
      (
        rm "$i" &&
          alert "Removed old OmegaGraf download: $i" ||
          die "Could not remove $i"
      )
  done

  is_file "OmegaGraf" &&
    (
      rm OmegaGraf &&
        alert "Removed old OmegaGraf binary" ||
        die "Could not remove OmegaGraf"
    )
}

permissions() {
  announce "Setting permissions"

  progress "Allowing [$USER] to use docker"
  sudo usermod -aG docker $USER &&
    success "[$USER] has been added to the docker group" ||
    die "Could not add [$USER] to the docker group"

  progress "Creating the data directory"
  mkdir -p data &&
    success "Created the data directory" ||
    die "Could not create the data directory"

  progress "Setting permissions on the data directory"
  chmod -R 777 data &&
    success "Set permissions on the data directory" ||
    die "Could not set permissions on the data directory"
}

enable_ssl() {
  announce "Enabling SSL"

  fail "Enabling SSL is currently not supported via this script, sorry :("
}

main() {
  announce "Starting the OmegaGraf installation helper"

  prep_log_and_temp_dir

  log "Program: $PROGFNAME $PROGVERS ($PROGUUID)"
  log "Updated: $PROGDATE"
  log "Run as : $USER@$HOSTNAME"

  prechecks

  dependencies

  cleanup

  download

  permissions

  if confirm "Would you like to generate a certificate and enable SSL?"; then
    OMEGAGRAF_HOST="http://0.0.0.0:5000"
    enable_ssl
  else
    OMEGAGRAF_HOST="http://0.0.0.0:5000"
    alert "Skipping SSL setup"
  fi

  announce "OmegaGraf is ready! To start it, run:\n   ./OmegaGraf --host $OMEGAGRAF_HOST"
}

#####################################################################

# set strict mode -  via http://redsymbol.net/articles/unofficial-bash-strict-mode/
# removed -e because it made basic [[ testing ]] difficult
set -uo pipefail
IFS=$'\n\t'
hash() {
  if [[ -n $(which md5sum) ]]; then
    # regular linux
    md5sum | cut -c1-6
  else
    # macos
    md5 | cut -c1-6
  fi
}

# change program version to your own release logic
readonly PROGNAME=$(basename "$0" .sh)
readonly PROGFNAME=$(basename "$0")
readonly PROGDIRREL=$(dirname "$0")
if [[ -z "$PROGDIRREL" ]]; then
  # script called without  path specified ; must be in $PATH somewhere
  readonly PROGFULLPATH=$(which "$0")
  readonly PROGDIR=$(dirname "$PROGFULLPATH")
else
  readonly PROGDIR=$(cd "$PROGDIRREL" && pwd)
  readonly PROGFULLPATH="$PROGDIR/$PROGFNAME"
fi
readonly PROGLINES=$(awk <"$PROGFULLPATH" 'END {print NR}')
readonly PROGHASH=$(hash <"$PROGFULLPATH")
readonly PROGUUID="L:${PROGLINES}-MD:${PROGHASH}"
# this is version of bash-boilerplate - replace by versioning of your script; start at 1.0.0
readonly TODAY=$(date "+%Y-%m-%d")
readonly PROGIDEN="«${PROGNAME} ${PROGVERS}»"
[[ -z "${TEMP:-}" ]] && TEMP=/tmp

PROGDATE="??"
os_uname=$(uname -s)
[[ "$os_uname" = "Linux" ]] && PROGDATE=$(stat -c %y "$0" 2>/dev/null | cut -c1-16) # generic linux
[[ "$os_uname" = "Darwin" ]] && PROGDATE=$(stat -f "%Sm" "$0" 2>/dev/null)          # for MacOS

if [[ -n "$(command -v yum)" ]]; then
  readonly PACKAGE_MANAGER="yum"
fi
if [[ -n "$(command -v apt-get)" ]]; then
  readonly PACKAGE_MANAGER="apt-get"
fi

verbose=0
quiet=0
piped=0
force=0
help=0
tmpd="$TEMP/$PROGNAME"
logd="./log"
tmpfile=$(mktemp "$tmpd/$TODAY.XXXXXX")
logfile=$logd/$PROGNAME.$TODAY.log

[[ $# -gt 0 ]] && [[ $1 == "-v" ]] && verbose=1
#to enable verbose even for option parsing

[[ -t 1 ]] && piped=0 || piped=1                                 # detect if out put is piped
[[ $(echo -e '\xe2\x82\xac') == '€' ]] && unicode=1 || unicode=0 # detect if unicode is supported

# Defaults

if [[ $piped -eq 0 ]]; then
  readonly col_reset="\033[0m"
  readonly col_red="\033[1;31m"
  readonly col_grn="\033[1;32m"
  readonly col_ylw="\033[1;33m"
else
  # no colors for piped content
  readonly col_reset=""
  readonly col_red=""
  readonly col_grn=""
  readonly col_ylw=""
fi

if [[ $unicode -gt 0 ]]; then
  readonly char_succ="✔"
  readonly char_fail="✖"
  readonly char_alrt="➨"
  readonly char_wait="…"
else
  # no unicode chars if not supported
  readonly char_succ="OK "
  readonly char_fail="!! "
  readonly char_alrt="?? "
  readonly char_wait="..."
fi

readonly nbcols=$(tput cols)
readonly wprogress=$((nbcols - 5))
#readonly nbrows=$(tput lines)

out() {
  echo "$(date '+%H:%M:%S') | [$PROGFNAME] $PROGVERS ($PROGUUID) $*" >>"$logfile"
  ((quiet)) && return
  local message="$*"
  local prefix=""
  if is_not_empty "$prefix_fmt"; then
    prefix=$(date "$prefix_fmt")
  fi
  printf '%b\n' "$prefix$message"
}
#TIP: use «out» to show any kind of output, except when option --quiet is specified
#TIP:> out "User is [$USER]"

progress() {
  ((quiet)) && return
  local message="$*"
  if ((piped)); then
    printf '%b\n' "$message"
    # \r makes no sense in file or pipe
  else
    printf "... %-${wprogress}b\r" "$message                                             "
    # next line will overwrite this line
  fi
}
#TIP: use «progress» to show one line of progress that will be overwritten by the next output
#TIP:> progress "Now generating file $nb of $total ..."

error_prefix="${col_red}>${col_reset}"
trap "die \"ERROR \$? after \$SECONDS seconds \n\
\${error_prefix} last command : '\$BASH_COMMAND' \" \
\$(< \$PROGFULLPATH awk -v lineno=\$LINENO \
'NR == lineno {print \"\${error_prefix} from line \" lineno \" : \" \$0}')" INT TERM EXIT
# cf https://askubuntu.com/questions/513932/what-is-the-bash-command-variable-good-for
# trap 'echo ‘$BASH_COMMAND’ failed with error code $?' ERR
safe_exit() {
  [[ -n "$tmpfile" ]] && [[ -f "$tmpfile" ]] && rm "$tmpfile"
  trap - INT TERM EXIT
  exit 0
}

is_set() { [[ "$1" -gt 0 ]]; }
is_empty() { [[ -z "$1" ]]; }
is_not_empty() { [[ -n "$1" ]]; }
#TIP: use «is_empty» and «is_not_empty» to test for variables
#TIP:> if ! confirm "Delete file"; then ; echo "skip deletion" ;   fi

is_file() { [[ -f "$1" ]]; }
is_dir() { [[ -d "$1" ]]; }

die() {
  tput bel
  out "${col_red}${char_fail} $PROGIDEN${col_reset}: $*" >&2
  safe_exit
}
fail() {
  tput bel
  out "${col_red}${char_fail} $PROGIDEN${col_reset}: $*" >&2
}
#TIP: use «die» to show error message and exit program
#TIP:> if [[ ! -f $output ]] ; then ; die "could not create output" ; fi

alert() {
  out "${col_red}${char_alrt}${col_reset}: $*" >&2
} # print error and continue
#TIP: use «alert» to show alert message but continue
#TIP:> if [[ ! -f $output ]] ; then ; alert "could not create output" ; fi

success() {
  out "${col_grn}${char_succ}${col_reset}  $*                                             "
}
#TIP: use «success» to show success message but continue
#TIP:> if [[ -f $output ]] ; then ; success "output was created!" ; fi

announce() {
  out "${col_grn}${char_wait}${col_reset}  $*"
  sleep 1
}
#TIP: use «announce» to show the start of a task
#TIP:> announce "now generating the reports"

log() {
  echo "$(date '+%H:%M:%S') | [$PROGFNAME] $PROGVERS ($PROGUUID) $*" >>"$logfile"
  is_set $verbose && out "${col_ylw}# $* ${col_reset}"
}
debug() { is_set $verbose && out "${col_ylw}# $* ${col_reset}"; }
#TIP: use «log» to show information that will only be visible when -v is specified
#TIP:> log "input file: [$inputname] - [$inputsize] MB"

escape() { echo "$*" | sed 's/\//\\\//g'; }
#TIP: use «escape» to extra escape '/' paths in regex
#TIP:> sed 's/$(escape $path)//g'

lcase() { echo "$*" | awk '{print tolower($0)}'; }
ucase() { echo "$*" | awk '{print toupper($0)}'; }
#TIP: use «lcase» and «ucase» to convert to upper/lower case
#TIP:> param=$(lcase $param)

confirm() {
  is_set $force && return 0
  read -r -p "$1 [y/N] " -n 1
  echo " "
  [[ $REPLY =~ ^[Yy]$ ]]
}
#TIP: use «confirm» for interactive confirmation before doing something
#TIP:> if ! confirm "Delete file"; then ; echo "skip deletion" ;   fi

ask() {
  # $1 = variable name
  # $2 = question
  # $3 = default value
  # not using read -i because that doesn't work on MacOS
  local ANSWER
  read -r -p "$2 ($3) > " ANSWER
  if [[ -z "$ANSWER" ]]; then
    eval "$1=\"$3\""
  else
    eval "$1=\"$ANSWER\""
  fi
}
#TIP: use «ask» for interactive setting of variables
#TIP:> ask NAME "What is your name" "Peter"

os_uname=$(uname -s)
os_bits=$(uname -m)
os_version=$(uname -v)

on_mac() { [[ "$os_uname" = "Darwin" ]]; }
on_linux() { [[ "$os_uname" = "Linux" ]]; }

on_32bit() { [[ "$os_bits" = "i386" ]]; }
on_64bit() { [[ "$os_bits" = "x86_64" ]]; }
#TIP: use «on_mac»/«on_linux»/'on_32bit'/'on_64bit' to only run things on certain platforms
#TIP:> on_mac && log "Running on MacOS"

verify_programs() {
  log "Running: on $os_uname ($os_version)"
  list_programs=$(echo "$*" | sort -u | tr "\n" " ")
  hash_programs=$(echo "$list_programs" | hash)
  verify_cache="$tmpd/.$PROGNAME.$hash_programs.verified"
  if [[ -f "$verify_cache" ]]; then
    log "Verify : $list_programs (cached)"
  else
    log "Verify : $list_programs"
    programs_ok=1
    for prog in "$@"; do
      if [[ -z $(which "$prog") ]]; then
        alert "$PROGIDEN needs [$prog] but this program cannot be found on this $os_uname machine"
        programs_ok=0
      fi
    done
    if [[ $programs_ok -eq 1 ]]; then
      (
        echo "$PROGNAME: check required programs OK"
        echo "$list_programs"
        date
      ) >"$verify_cache"
    fi
  fi
}

folder_prep() {
  if [[ -n "$1" ]]; then
    local folder="$1"
    local maxdays=365
    if [[ -n "$2" ]]; then
      maxdays=$2
    fi
    if [ ! -d "$folder" ]; then
      log "Create folder [$folder]"
      mkdir "$folder"
    else
      log "Cleanup: [$folder] - delete files older than $maxdays day(s)"
      find "$folder" -mtime "+$maxdays" -type f -exec rm {} \;
    fi
  fi
}
#TIP: use «folder_prep» to create a folder if needed and otherwise clean up old files
#TIP:> folder_prep "$logd" 7 # delete all files olders than 7 days

prep_log_and_temp_dir() {
  if [[ -n "$tmpd" ]]; then
    folder_prep "$tmpd" 1
    log "Tmpfile: $tmpfile"
    # you can use this teporary file in your program
    # it will be deleted automatically if the program ends without problems
  fi
  if [[ -n "$logd" ]]; then
    folder_prep "$logd" 7
    log "Logfile: $logfile"
  fi
}
[[ $runasroot == 1 ]] && [[ $UID -ne 0 ]] && die "MUST be root to run this script"
[[ $runasroot == -1 ]] && [[ $UID -eq 0 ]] && die "CANNOT be root to run this script"

# this will show up even if your main() has errors
log "-------- STARTING (main) $PROGIDEN"
main
# main program is finished
log "-------- FINISH   (main) $PROGIDEN"
safe_exit
