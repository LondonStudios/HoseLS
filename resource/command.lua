-- The purpose of this file is for server owners and developers to add permission checks
print("registered")
RegisterCommand("hose", function(source, args, rawCommand)
    print("command")
    if (source > 0) then
        -- Add your permission checks here.
        -- The boolean value in the command below is whether they have permission or not.
        -- This will notify them if they do not have permission.
        TriggerClientEvent("Client:HoseCommand", source, true)
        print("test")
    end
end, false)