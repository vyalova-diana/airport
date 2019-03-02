require 'sinatra'
require 'sequel'
require 'json'

DB = Sequel.sqlite #TODO: Change to on-disk
DB.create_table :passengers do
  primary_key :id
  Int :global_id
end

DB.create_table :cargos do
  primary_key :id
  Int :global_id
  Int :passenger_id
end

post '/passenger/:id' do |id|
  if (DB[:passengers].first(:global_id => id) == nil) then
    DB[:passengers].insert(:global_id => id)
    ""
  else 
    "{error: 1}"
  end
end

post '/cargos/:id' do |id|
  data = JSON.parse request.body.read
  data["cargos"].each { |cargo|
    print(cargo)
    DB[:cargos].insert(:passenger_id => id, :global_id => cargo["global_id"])
  }
  ""
end

get '/take/:id' do |id|
  passenger = DB[:passengers].first(:id => id)
  if (passenger != nil) then
    passenger = DB[:passengers].first(:id => id)
    DB[:passengers].where(:global_id => passenger[:global_id]).delete
    cargos = DB[:cargos].where(:passenger_id => id)
    cargos_arr = []
    cargos.each do |hash|
      cargo_hash = { :global_id => hash[:global_id] }
      cargos_arr << cargo_hash
    end
    cargos.each { |cargo|
      DB[:cargos].where(:global_id => cargo[:global_id]).delete
    }
    cargo_json = cargos_arr.to_json
    "{id:#{id}, cargos: #{cargo_json}"
  else
    "{error: 2}"
  end
end