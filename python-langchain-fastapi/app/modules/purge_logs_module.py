import os
import time

# Path to the log file
LOG_FILE = os.path.join(os.path.dirname(os.path.abspath(__file__)), "audit.log")

# Purge time limit (7 days in seconds)
PURGE_DAYS = 7
PURGE_TIME = PURGE_DAYS * 24 * 60 * 60  # Convert to seconds

def purge_logs():
    """Delete log lines older than 7 days"""
    if not os.path.exists(LOG_FILE):
        print("No log found.")
        return

    # Read all log lines
    with open(LOG_FILE, "r") as file:
        lines = file.readlines()

    # Filter recent lines (last 7 days)
    now = time.time()
    recent_lines = [line for line in lines if "AUDIT LOG:" in line and check_date(line, now)]

    # Overwrite the file with only recent logs
    with open(LOG_FILE, "w") as file:
        file.writelines(recent_lines)

    print(f"Old logs have been removed. {len(recent_lines)} records remain.")

def check_date(line, current_time):
    """Extract the log date and check if it is within the last 7 days"""
    try:
        date_str = line.split(" - ")[0]  # Get the date in the format YYYY-MM-DD HH:MM:SS
        log_date = time.mktime(time.strptime(date_str, "%Y-%m-%d %H:%M:%S"))
        return current_time - log_date < PURGE_TIME  # Return True if within the last 7 days
    except Exception:
        return True  # Keep the line if there is an error in the conversion

if __name__ == "__main__":
    purge_logs()