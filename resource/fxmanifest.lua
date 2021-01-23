fx_version 'bodacious'
games { 'gta5' }

author 'London Studios & Adam Fenton'
description 'A resource providing a realistic firefighting experience, featuring a water hose.'

client_scripts {
    'Client.net.dll',
}

server_scripts {
    'command.lua',
    'Server.net.dll',
}

files {
    'hose/contentunlocks.meta',
	'hose/loadouts.meta',
	'hose/pedpersonality.meta',
	'hose/shop_weapon.meta',
	'hose/weaponanimations.meta',
	'hose/weaponarchetypes.meta',
	'hose/weapons.meta',
}

data_file 'WEAPONINFO_FILE' 'hose/weapons.meta'
data_file 'WEAPON_METADATA_FILE' 'hose/weaponarchetypes.meta'
data_file 'WEAPON_SHOP_INFO' 'hose/shop_weapon.meta'
data_file 'WEAPON_ANIMATIONS_FILE' 'hose/weaponanimations.meta'
data_file 'CONTENT_UNLOCKING_META_FILE' 'hose/contentunlocks.meta'
data_file 'LOADOUTS_FILE' 'hose/loadouts.meta'
data_file 'PED_PERSONALITY_FILE' 'hose/pedpersonality.meta'

-- Join the London Studios Discord here: https://discord.gg/F2zmUTD

-- Purchase the new Supply Line to integrate in with HoseLS here: https://store.londonstudios.net/category/1704957
-- The Supply Line resource removes the infinite water supply, requiring you to setup supply lines with fire vehicles